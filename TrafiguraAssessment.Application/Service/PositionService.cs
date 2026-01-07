using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafiguraAssessment.Application._Enum;
using TrafiguraAssessment.Application.DTO;
using TrafiguraAssessment.Application.Interface;
using TrafiguraAssessment.Domain.Entities;
using TrafiguraAssessment.Infrastructure.UnitOfWork;

namespace TrafiguraAssessment.Application.Service
{
    public class PositionService : IPositionService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        private readonly Dictionary<int, SortedList<int, TradeTransaction>> _trades = new();
       
        public PositionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _uow = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddTransactionAsync(TransactionDto transactionDto)
        {
            // 1. Check last trade for this security
            var lastTrade = _uow.Repository<TradeTransaction>().Query()
                .Where(t => t.SecurityCode == transactionDto.SecurityCode)
                .OrderByDescending(t => t.Version)
                .FirstOrDefault();

            int tradeId;
            int version;

            if (lastTrade == null)
            {
                // New trade for this security
                tradeId = (_uow.Repository<TradeTransaction>().Query().Max(t => (int?)t.TradeId) ?? 0) + 1;
                version = 1;
            }
            else
            {
                tradeId = lastTrade.TradeId;
                version = lastTrade.Version + 1;
            }

            // 2. Map DTO
            var txn = _mapper.Map<TradeTransaction>(transactionDto);

            // 3. Set calculated TradeId and Version
            txn.TradeId = tradeId;
            txn.Version = version;

            // 4. Save to DB
            await _uow.Repository<TradeTransaction>().AddAsync(txn);
            _uow.Save();

            // 5. Update in-memory _trades dictionary
            if (!_trades.ContainsKey(txn.TradeId))
                _trades[txn.TradeId] = new SortedList<int, TradeTransaction>();

            _trades[txn.TradeId][txn.Version] = txn;
         
        }

        public async Task<Dictionary<string, int>> GetPositionsAsync()
        {
            
            var allTxn = await _uow.Repository<TradeTransaction>().GetAllAsync();

            var groupedByTrade = allTxn.GroupBy(t => t.TradeId);

            var positions = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            // Collect all security codes encountered
            var allSecurityCodes = new HashSet<string>(allTxn.Select(t => t.SecurityCode), StringComparer.OrdinalIgnoreCase);

            foreach (var group in groupedByTrade)
            {
                var highestVersion = group.OrderBy(t => t.Version).Last();

                if (highestVersion.Action.Equals("CANCEL", StringComparison.OrdinalIgnoreCase))
                    continue; // zero contribution

                int qty = highestVersion.BuySell == TransactionType.BUY.ToString()
                    ? highestVersion.Quantity
                    : -highestVersion.Quantity;

                if (!positions.ContainsKey(highestVersion.SecurityCode))
                    positions[highestVersion.SecurityCode] = 0;

                positions[highestVersion.SecurityCode] += qty;
            }

            // Ensure all security codes are present with zero if missing
            foreach (var sec in allSecurityCodes)
            {
                if (!positions.ContainsKey(sec))
                    positions[sec] = 0;
            }

            return positions;
        }
    }
}

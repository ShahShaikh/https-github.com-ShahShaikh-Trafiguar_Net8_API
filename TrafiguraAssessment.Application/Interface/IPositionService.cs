using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafiguraAssessment.Application.DTO;

namespace TrafiguraAssessment.Application.Interface
{
    public interface IPositionService
    {
        Task AddTransactionAsync(TransactionDto transactionDto);
        Task<Dictionary<string, int>> GetPositionsAsync();
    }
}

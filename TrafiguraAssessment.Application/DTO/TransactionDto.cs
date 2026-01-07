using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafiguraAssessment.Application.DTO
{
    public class TransactionDto
    {
        public int TradeID { get; set; }
        public int Version { get; set; }
        public string SecurityCode { get; set; } = null!;
        public int Quantity { get; set; }
        public string Action { get; set; } = null!;
        public string BuySell { get; set; } = null!;
    }
}

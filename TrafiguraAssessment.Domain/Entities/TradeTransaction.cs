using System;
using System.Collections.Generic;

namespace TrafiguraAssessment.Domain.Entities;

public partial class TradeTransaction
{
    public int TransactionId { get; set; }

    public int TradeId { get; set; }

    public int Version { get; set; }

    public string SecurityCode { get; set; } = null!;

    public int Quantity { get; set; }

    public string Action { get; set; } = null!;

    public string BuySell { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}

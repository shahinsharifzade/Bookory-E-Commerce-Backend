using Bookory.Core.Models.Common;

namespace Bookory.Core.Models;

public class PaymentDetail : BaseEntity //Stripe
{
    public decimal Amount { get; set; }
    public string? Status { get; set; }
    public string? TransactionId { get; set; }
}

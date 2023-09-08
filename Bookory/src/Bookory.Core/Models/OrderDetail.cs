using Bookory.Core.Models.Common;
using Bookory.Core.Models.Identity;

namespace Bookory.Core.Models;

public class OrderDetail : BaseEntity
{
    public decimal Total { get; set; }

    public string? UserId { get; set; }
    public AppUser User { get; set; }

    public Guid PaymentDetailId { get; set; } //Stripe
    public PaymentDetail? PaymentDetail { get; set; } //Stripe

    public ICollection<OrderItem>? OrderItems { get; set; }

    public OrderDetail()
    {
        OrderItems = new List<OrderItem>();
    }
}

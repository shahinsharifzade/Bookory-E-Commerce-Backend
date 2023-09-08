using Bookory.Core.Models.Common;

namespace Bookory.Core.Models;

public class OrderItem : BaseEntity
{
    public int Quantity { get; set; }

    public Guid OrderDetailId { get; set; }
    public OrderDetail OrderDetail { get; set; }

    public Guid BookId { get; set; }
    public Book Book { get; set; }
}

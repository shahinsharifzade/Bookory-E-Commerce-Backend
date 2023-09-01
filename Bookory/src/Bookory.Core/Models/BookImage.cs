using Bookory.Core.Models.Common;

namespace Bookory.Core.Models;

public class BookImage : BaseEntity
{
    public string Image { get; set; }
    public bool IsMain { get; set; }
    public Guid BookId { get; set; }
    public Book Book { get; set; }
}

using Bookory.Core.Models.Common;
namespace Bookory.Core.Models;

public class BookGenre : BaseEntity
{
    public Guid GenreId { get; set; }
    public Genre Genre { get; set; }
    public Guid BookId { get; set; }
    public Book Book { get; set; }
}

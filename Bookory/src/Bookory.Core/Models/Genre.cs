using Bookory.Core.Models.Common;

namespace Bookory.Core.Models;

public class Genre : BaseEntity
{
    public string Name { get; set; }
    public ICollection<BookGenre> BookGenres { get; set; } = new List<BookGenre>();

}

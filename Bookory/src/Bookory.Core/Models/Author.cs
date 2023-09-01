using Bookory.Core.Models.Common;
namespace Bookory.Core.Models;

public class Author : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Biography { get; set; } = null!;
    public ICollection<AuthorImage> Images { get; set; } = new List<AuthorImage>();
    public ICollection<Book>? Books { get; set; } = new List<Book>();

}

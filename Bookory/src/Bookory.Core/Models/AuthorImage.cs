using Bookory.Core.Models.Common;

namespace Bookory.Core.Models;

public class AuthorImage : BaseEntity
{
    public string Image { get; set; }
    public bool IsMain { get; set; }
    public Guid AuthorId { get; set; }
    public Author Author { get; set; }
}

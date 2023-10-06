using Bookory.Core.Models.Common;
namespace Bookory.Core.Models;

public class Blog : BaseEntity
{
    public string Image { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public ICollection<Category> Categories { get; set; }
}

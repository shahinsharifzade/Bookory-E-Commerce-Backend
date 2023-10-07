using Bookory.Core.Models.Common;

namespace Bookory.Core.Models;

public class Category : BaseEntity
{
    public string Name { get; set; }
    public ICollection<Blog> Blogs { get; set; }

    public Category()
    {
        Blogs= new List<Blog>();
    }
}

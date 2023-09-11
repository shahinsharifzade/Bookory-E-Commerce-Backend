using Bookory.Core.Models.Common;
using Bookory.Core.Models.Identity;

namespace Bookory.Core.Models;

public class Comment : BaseEntity
{
    public string Content { get; set; }

    public string UserId { get; set; }
    public AppUser User { get; set; }

    public Guid? RefId { get; set; }
    public Comment? RefComment { get; set; }

    public Guid EntityId { get; set; }
    public string EntityType { get; set; }

    public ICollection<Comment> Replies { get; set; }
    public Comment()
    {
        Replies = new List<Comment>();
    }
}

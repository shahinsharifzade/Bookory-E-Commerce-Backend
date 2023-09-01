namespace Bookory.Core.Models.Common;

public class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreateBy { get; set; }

    public DateTime ModifiedAt { get; set;}
    public string? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

}

using Bookory.Core.Models.Common;

namespace Bookory.Core.Models;

public class Contact : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Message { get; set; } = null!;
}

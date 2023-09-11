using Bookory.Core.Models;
using Bookory.DataAccess.Persistance.Context.EfCore;
using Bookory.DataAccess.Repositories.Interfaces;
namespace Bookory.DataAccess.Repositories.Implementations;

public class CommentRepository : Repository<Comment>, ICommentRepository 
{
	public CommentRepository(AppDbContext context ) : base(context)
	{

	}
}

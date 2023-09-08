using Bookory.Core.Models;
using Bookory.DataAccess.Persistance.Context.EfCore;
using Bookory.DataAccess.Repositories.Interfaces;

namespace Bookory.DataAccess.Repositories.Implementations;

public class PaymentDetailRepository : Repository<PaymentDetail> , IPaymentDetailRepository
{
	public PaymentDetailRepository(AppDbContext context) : base(context)
	{

	}
}

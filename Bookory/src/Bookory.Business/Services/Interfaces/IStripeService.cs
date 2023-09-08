namespace Bookory.Business.Services.Interfaces;

public interface IStripeService
{
    //Task<CustomerResource> CreateCustomer(CustomerResourcePostDto resource, CancellationToken cancellationToken);
    //Task<ChargeResource> CreateCharge(ChargeResourcePostDto resource, CancellationToken cancellationToken);
    Task<string> ChargeAsync(string stripeEmail, string stripeToken, decimal totalAmount);

}

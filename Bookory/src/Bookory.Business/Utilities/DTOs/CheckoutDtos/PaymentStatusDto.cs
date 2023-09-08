namespace Bookory.Business.Utilities.DTOs.CheckoutDtos;

public class PaymentStatusDto
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public decimal? Amount { get; set; }
    public string? BalanceTransaction { get; set; }
}
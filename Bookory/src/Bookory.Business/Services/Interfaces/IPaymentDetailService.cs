using Bookory.Business.Utilities.DTOs.PaymentDetailDto;

namespace Bookory.Business.Services.Interfaces;

public interface IPaymentDetailService
{
    Task<PaymentDetailGetResponseDto> CreatePaymentDetailAsync(PaymentDetailPostDto paymentDetailPostDto);
}

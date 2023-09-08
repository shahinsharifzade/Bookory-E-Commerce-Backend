using AutoMapper;
using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.PaymentDetailDto;
using Bookory.Core.Models;
using Bookory.DataAccess.Repositories.Interfaces;

namespace Bookory.Business.Services.Implementations;

public class PaymentDetailService : IPaymentDetailService
{
    private readonly IMapper _mapper;
    private readonly IPaymentDetailRepository _paymentDetailRepository;
    public PaymentDetailService(IMapper mapper, IPaymentDetailRepository paymentDetailRepository)
    {
        _mapper = mapper;
        _paymentDetailRepository = paymentDetailRepository;
    }

    public async Task<PaymentDetailGetResponseDto> CreatePaymentDetailAsync(PaymentDetailPostDto paymentDetailPostDto)
    {
        var newPayment = _mapper.Map<PaymentDetail>(paymentDetailPostDto);
         
        await _paymentDetailRepository.CreateAsync(newPayment);
        await _paymentDetailRepository.SaveAsync();

        PaymentDetailGetResponseDto paymentDto = _mapper.Map<PaymentDetailGetResponseDto>(newPayment);
        return paymentDto;
    }
}

using AutoMapper;
using Bookory.Business.Utilities.DTOs.PaymentDetailDto;
using Bookory.Core.Models;

namespace Bookory.Business.Utilities.Mapper
{
    public class PaymentDetailMapperProfile : Profile
    {
        public PaymentDetailMapperProfile()
        {
            CreateMap<PaymentDetail, PaymentDetailGetResponseDto>().ReverseMap();
            CreateMap<PaymentDetailPostDto, PaymentDetail>();
        }
    }
}

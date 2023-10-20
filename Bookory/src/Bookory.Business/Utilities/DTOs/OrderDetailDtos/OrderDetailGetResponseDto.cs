using Bookory.Business.Utilities.DTOs.OrderItemDtos;
using Bookory.Business.Utilities.DTOs.UserAddressDtos;
using Bookory.Business.Utilities.DTOs.UserDtos;
using Bookory.Core.Models;

namespace Bookory.Business.Utilities.DTOs.OrderDetailDtos;

public record OrderDetailGetResponseDto(Guid Id, decimal Total, DateTime CreatedAt, UserGetResponseDto User, UserAddressGetReponseDto Useraddress, ICollection<OrderItemGetResponseDto>? OrderItems);
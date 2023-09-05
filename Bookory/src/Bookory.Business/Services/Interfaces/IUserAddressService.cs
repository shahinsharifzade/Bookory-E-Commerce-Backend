using Bookory.Business.Utilities.DTOs.Common;
using Bookory.Business.Utilities.DTOs.UserAddressDtos;
using Bookory.Core.Models;

namespace Bookory.Business.Services.Interfaces;

public interface IUserAddressService
{
    public Task<List<UserAddressGetReponseDto>> GetAllAddressAsync();
    public Task<UserAddressGetReponseDto> GetAddressByIdAsync(Guid id);
    public Task<ResponseDto> AddAddressAsync(UserAddressPostDto userAddressPostDto);
    public Task<ResponseDto> UpdateAddressAsync(UserAddressPutDto userAddressPutDto);
    public Task<ResponseDto> DeleteAddressAsync(Guid id);
}

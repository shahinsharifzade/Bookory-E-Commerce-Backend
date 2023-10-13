using Bookory.Business.Utilities.DTOs.BlogDtos;
using Bookory.Business.Utilities.DTOs.Common;
using Bookory.Business.Utilities.DTOs.ContactDtos;

namespace Bookory.Business.Services.Interfaces;

public interface IContactService 
{
    Task<List<ContactGetResponseDto>> GetAllContactMessagesAsync(string? search);
    Task<ContactGetResponseDto> GetContactMessageByIdAsync(Guid id);
    Task<ResponseDto> CreateContactMessageAsync(ContactPostDto blogPostDto);
    Task<ResponseDto> DeleteContactMessagesAsync(Guid id);
}

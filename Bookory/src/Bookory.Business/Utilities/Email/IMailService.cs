using Bookory.Business.Utilities.DTOs.MailDtos;

namespace Bookory.Business.Utilities.Email;

public interface IMailService
{
    Task SendEmailAsync(MailRequestDto mailRequesDto);
}

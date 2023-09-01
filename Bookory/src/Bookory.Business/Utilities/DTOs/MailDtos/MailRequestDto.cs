using Microsoft.AspNetCore.Http;
namespace Bookory.Business.Utilities.DTOs.MailDtos;

public record MailRequestDto(string ToEmail , string Subject , string Body , List<IFormFile>? Attachments);
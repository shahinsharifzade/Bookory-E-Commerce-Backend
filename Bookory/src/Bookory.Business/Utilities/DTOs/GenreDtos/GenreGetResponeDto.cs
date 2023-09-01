using Bookory.Business.Utilities.DTOs.BookDtos;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Bookory.Business.Utilities.DTOs.GenreDtos;
public record GenreGetResponeDto(Guid Id, string Name, List<BookGetResponseDto> Books);

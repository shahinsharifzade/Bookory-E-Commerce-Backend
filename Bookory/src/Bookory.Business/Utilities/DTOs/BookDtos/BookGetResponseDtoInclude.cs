﻿using Bookory.Business.Utilities.DTOs.BookImageDtos;

namespace Bookory.Business.Utilities.DTOs.BookDtos;

public record BookGetResponseDtoInclude(Guid Id, string Title, string Description, string MainImage, decimal Price, decimal DiscountPrice, decimal Rating, ICollection<BookImageGetResponseDtoInclude> Images ); 
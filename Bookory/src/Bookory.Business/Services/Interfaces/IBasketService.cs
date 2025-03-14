﻿using Bookory.Business.Utilities.DTOs.BasketDtos;
using Bookory.Business.Utilities.DTOs.Common;

namespace Bookory.Business.Services.Interfaces;

public interface IBasketService
{
    public Task<List<BasketGetResponseDto>> GetBasketItemAsync(string id);
    public Task<ResponseDto> AddItemToBasketAsync(BasketPostDto basketPostDto);
    public Task<ResponseDto> UpdateItemAsync(BasketPutDto basketPutDto);
    public Task<ResponseDto> RemoveBasketItemAsync(Guid id);
    public Task<List<BasketGetResponseDto>> GetActiveUserBasket();
    public Task<ResponseDto> TransferCookieBasketToDatabaseAsync(string userId);
}

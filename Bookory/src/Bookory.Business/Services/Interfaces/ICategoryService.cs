using Bookory.Business.Utilities.DTOs.CategoryDtos;
using Bookory.Business.Utilities.DTOs.Common;
using Bookory.Core.Models;

namespace Bookory.Business.Services.Interfaces;

public interface ICategoryService
{
    Task<List<CategoryGetReponseDto>> GetAllCategoriesAsync(string? search);
    Task<CategoryGetReponseDto> GetCategoryByIdAsync(Guid id);
    Task<ResponseDto> CreateCategoryAsync(CategoryPostDto categoryPostDto);
    Task<ResponseDto> UpdateCategoryAsync(CategoryPutDto categoryPutDto);
    Task<ResponseDto> DeleteCategoryAsync(Guid id);
    Task<Category> CategoryAllDetailsGetByIdAsync(Guid id);

}

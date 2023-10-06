using AutoMapper;
using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.CategoryDtos;
using Bookory.Business.Utilities.DTOs.Common;
using Bookory.Business.Utilities.Exceptions.CategoryException;
using Bookory.Core.Models;
using Bookory.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Bookory.Business.Services.Implementations;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }


    public async Task<List<CategoryGetReponseDto>> GetAllCategoriesAsync(string? search)
    {
        var categories = await _categoryRepository.GetFiltered(
    c => string.IsNullOrEmpty(search) ? true : c.Name.ToLower().Contains(search.Trim().ToLower())).ToListAsync();

        if (categories == null || categories.Count == 0)
            throw new CategoryNotFoundException("No categories found matching the search criteria.");

        var categoriesDto = _mapper.Map<List<CategoryGetReponseDto>>(categories);
        return categoriesDto;
    }

    public async Task<CategoryGetReponseDto> GetCategoryByIdAsync(Guid id)
    {
        var category = await _categoryRepository.GetSingleAsync(g => g.Id == id);

        if (category is null)
            throw new CategoryNotFoundException($"Category with the specified ID '{id}' was not found");

        var categoryDto = _mapper.Map<CategoryGetReponseDto>(category);
        return categoryDto;
    }

    public async Task<ResponseDto> CreateCategoryAsync(CategoryPostDto categoryPostDto)
    {
        bool isExist = await _categoryRepository.IsExistAsync(g => g.Name.ToLower().Trim() == categoryPostDto.Name.ToLower().Trim());

        if (isExist)
            throw new CategoryAlreadyExistExeption($"A category with the title '{categoryPostDto.Name}' already exists. Please choose a different title");

        var newCategory = _mapper.Map<Category>(categoryPostDto);

        await _categoryRepository.CreateAsync(newCategory);
        await _categoryRepository.SaveAsync();

        return new((int)HttpStatusCode.Created, "Category successfully created");
    }


    public async Task<ResponseDto> UpdateCategoryAsync(CategoryPutDto categoryPutDto)
    {
        bool isExist = await _categoryRepository.IsExistAsync(g => g.Name.ToLower().Trim() == categoryPutDto.Name.ToLower().Trim() && g.Id != categoryPutDto.Id);
        if (isExist)
            throw new CategoryAlreadyExistExeption($"A category with the title '{categoryPutDto.Name}' already exists");

        var category = await _categoryRepository.GetSingleAsync(g => g.Id == categoryPutDto.Id);
        if (category is null)
            throw new CategoryNotFoundException($"Category with ID '{categoryPutDto.Id}' not found");

        var newCategory = _mapper.Map(categoryPutDto, category);
        _categoryRepository.Update(newCategory);
        await _categoryRepository.SaveAsync();

        return new((int)HttpStatusCode.OK, "Category updated successfully");
    }

    public async Task<ResponseDto> DeleteCategoryAsync(Guid id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category is null)
            throw new CategoryNotFoundException($"No category found with the ID: {id}");

        _categoryRepository.SoftDelete(category);
        await _categoryRepository.SaveAsync();
        return new((int)HttpStatusCode.OK, "Category successfully deleted");
    }
}

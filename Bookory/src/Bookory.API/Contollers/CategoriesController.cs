using Bookory.Business.Services.Implementations;
using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.CategoryDtos;
using Bookory.Business.Utilities.Enums;
using Bookory.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookory.API.Contollers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search)
    {
        return Ok(await _categoryService.GetAllCategoriesAsync(search));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        return Ok(await _categoryService.GetCategoryByIdAsync(id));
    }

    [HttpPost]
    [Authorize(Roles = "Admin, Moderator")]
    public async Task<IActionResult> Post(CategoryPostDto categoryPostDto)
    {
        var response = await _categoryService.CreateCategoryAsync(categoryPostDto);

        return StatusCode(response.StatusCode, response.Message);
    }

    [HttpPut]
    [Authorize(Roles = "Admin, Moderator")]
    public async Task<IActionResult> Put(CategoryPutDto categoryPutDto)
    {
        var response = await _categoryService.UpdateCategoryAsync(categoryPutDto);

        return StatusCode(response.StatusCode, response.Message);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin, Moderator")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _categoryService.DeleteCategoryAsync(id);

        return StatusCode(response.StatusCode, response.Message);
    }
}

using AutoMapper;
using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.BlogDtos;
using Bookory.Business.Utilities.DTOs.BookDtos;
using Bookory.Business.Utilities.DTOs.Common;
using Bookory.Business.Utilities.Exceptions.BlogException;
using Bookory.Business.Utilities.Exceptions.BookExceptions;
using Bookory.Core.Models;
using Bookory.DataAccess.Repositories.Implementations;
using Bookory.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using MimeKit.Cryptography;
using System.Net;

namespace Bookory.Business.Services.Implementations;

public class BlogService : IBlogService
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ICategoryService _categoryService;
    private readonly IBlogRepository _blogRepository;
    private readonly IMapper _mapper;

    public BlogService(IBlogRepository blogRepository, IMapper mapper, IWebHostEnvironment webHostEnvironment, ICategoryService categoryService)
    {
        _blogRepository = blogRepository;
        _mapper = mapper;
        _webHostEnvironment = webHostEnvironment;
        _categoryService = categoryService;
    }


    public async Task<List<BlogGetResponseDto>> GetAllBlogsAsync(string? search)
    {
        var blogs = await _blogRepository.GetFiltered(
          b => string.IsNullOrEmpty(search) || b.Title.ToLower().Contains(search.Trim().ToLower()), nameof(Blog.Categories)).ToListAsync();

        if (blogs == null || blogs.Count == 0)
            throw new BlogNotFoundException("No blogs were found matching the search criteria.");

        var blogDtos = _mapper.Map<List<BlogGetResponseDto>>(blogs);
        return blogDtos;
    }

    public async Task<BlogGetResponseDto> GetBlogByIdAsync(Guid id)
    {
        var blog = await _blogRepository.GetSingleAsync(b => b.Id == id, nameof(Blog.Categories));

        if (blog is null)
            throw new BlogNotFoundException($"The blog with ID {id} was not found");

        var blogDtos = _mapper.Map<BlogGetResponseDto>(blog);
        return blogDtos;
    }

    public async Task<ResponseDto> CreateBlogAsync(BlogPostDto blogPostDto)
    {
        bool isExist = await _blogRepository.IsExistAsync(b => b.Title.ToLower().Trim() == blogPostDto.Title.ToLower().Trim());
        if (isExist)
            throw new BlogAlreadyExistException($"A blog with the title '{blogPostDto.Title}' already exists");

        var newBlog = _mapper.Map<Blog>(blogPostDto);

        List<Category> categories = new();

        foreach (var categoryId in blogPostDto.CategoryIds)
        {
            var category = await _categoryService.CategoryAllDetailsGetByIdAsync(categoryId);
            categories.Add(category);
        }

        newBlog.Categories = categories;

        await _blogRepository.CreateAsync(newBlog);
        await _blogRepository.SaveAsync();

        return new((int)HttpStatusCode.Created, "The blog has been successfully created");
    }

    public async Task<ResponseDto> UpdateBlogAsync(BlogPutDto blogPutDto)
    {
        var existingBlog = await _blogRepository.GetByIdAsync(blogPutDto.Id, nameof(Blog.Categories));
        if (existingBlog == null)
            throw new BlogNotFoundException($"Blog with ID {blogPutDto.Id} not found");

        bool isExist = await _blogRepository.IsExistAsync(b =>
            b.Id != blogPutDto.Id && b.Title.ToLower().Trim() == blogPutDto.Title.ToLower().Trim());
        if (isExist)
            throw new BlogAlreadyExistException($"A blog with the title '{blogPutDto.Title}' already exists");

        existingBlog.Categories.Clear();

        var updatedBook = _mapper.Map(blogPutDto, existingBlog);

        var updatedCategories = new List<Category>();
        foreach (var categoryId in blogPutDto.CategoryIds)
        {
            var category = await _categoryService.CategoryAllDetailsGetByIdAsync(categoryId);
            if (category != null)
                updatedCategories.Add(category);
        }
        updatedBook.Categories = updatedCategories;

        _blogRepository.Update(updatedBook);
        await _blogRepository.SaveAsync();

        return new ResponseDto((int)HttpStatusCode.OK, "The blog has been successfully updated");
    }

    public async Task<ResponseDto> DeleteBlogAsync(Guid id)
    {
        var blog = await _blogRepository.GetSingleAsync(b => b.Id == id);
        if (blog is null)
            throw new BlogNotFoundException($"The book with ID {id} was not found");

        _blogRepository.SoftDelete(blog);
        await _blogRepository.SaveAsync();

        return new((int)HttpStatusCode.OK, "The book has been successfully deleted");
    }

    public async Task<BlogPageResponseDto> GetFilteredBlogsAsync(int pageNumber, int pageSize, BlogFiltersDto filters)
    {
        var blogsQuery = _blogRepository.GetFiltered(b => (string.IsNullOrEmpty(filters.Search) || b.Title.ToLower().Contains(filters.Search.Trim().ToLower())), nameof(Blog.Categories));

        blogsQuery = blogsQuery.OrderByDescending(b => b.CreatedAt);

        if (filters.Categories != null && filters.Categories.Any() )
            blogsQuery = blogsQuery.Where(b => b.Categories.Any(c => filters.Categories.Contains(c.Id)));

        if (filters.SortBy != null)
            switch (filters.SortBy)
            {
                case "newest":
                    blogsQuery = blogsQuery.OrderByDescending(b => b.CreatedAt);
                    break;
            }

        decimal totalCount = await blogsQuery.CountAsync();

        if (pageSize != 0)
        {
            totalCount = Math.Ceiling((decimal)await blogsQuery.CountAsync() / pageSize);
        }

        int itemsToSkip = (pageNumber - 1) * pageSize;
        blogsQuery = blogsQuery.Skip(itemsToSkip).Take(pageSize);

        var blogs = await blogsQuery.ToListAsync();

        if (blogs is null || blogs.Count == 0)
            throw new BlogNotFoundException("No blogs were found matching the provided criteria.");

        var blogsGetResponseDto = _mapper.Map<List<BlogGetResponseDto>>(blogs);

        return new BlogPageResponseDto(blogsGetResponseDto, totalCount);
    }
}

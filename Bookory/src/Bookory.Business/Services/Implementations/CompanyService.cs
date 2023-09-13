using AutoMapper;
using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.Common;
using Bookory.Business.Utilities.DTOs.CompanyDtos;
using Bookory.Business.Utilities.Enums;
using Bookory.Business.Utilities.Extension.FileExtensions.Common;
using Bookory.Core.Models;
using Bookory.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Bookory.Business.Services.Implementations;

public class CompanyService : ICompanyService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IBookService _bookService;
    private readonly ICompanyRepository _companyRepository;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public CompanyService(IMapper mapper, IHttpContextAccessor httpContextAccessor, IUserService userService, ICompanyRepository companyRepository, IWebHostEnvironment webHostEnvironment)
    {
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _userService = userService;
        _companyRepository = companyRepository;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<ResponseDto> CreateCompanyAsync(CompanyPostDto companyPostDto)
    {
        var userDetails = await _userService.GetUserByUsernameAsync(companyPostDto.Username);

        if (userDetails.Role != Roles.Vendor.ToString()) throw new Exception("Olmas");
        if (userDetails.User.IsVendorRegistrationComplete == true) throw new Exception("Olmas dedim");

        var company = _mapper.Map<Company>(companyPostDto);
        company.UserId = userDetails.User.Id;
        company.Status = CompanyStatus.PendingApproval;
        //userDetails.User.CompanyId

        await _companyRepository.CreateAsync(company);
        await _companyRepository.SaveAsync();

        userDetails.User.CompanyId = company.Id;
        await _userService.UpdateUserAsync(userDetails.User);

        return new((int)HttpStatusCode.Created, "Company created and is pending admin approval");
    }

    public async Task<List<CompanyGetResponseDto>> GetAllCompaniesAsync(string? search)
    {
        var companies = await _companyRepository.GetFiltered(
                c => (string.IsNullOrEmpty(search) || c.Name.ToLower().Contains(search.Trim().ToLower())) && c.Status == CompanyStatus.Approved, includes).ToListAsync();
        //c => (string.IsNullOrEmpty(search) || c.Name.ToLower().Contains(search.Trim().ToLower()))).ToListAsync();

        var companiesDto = _mapper.Map<List<CompanyGetResponseDto>>(companies);

        if (companies is null || companies.Count == 0) throw new Exception("Empty");

        return companiesDto;
    }

    public async Task<CompanyGetResponseDto> GetCompanyByIdAsync(Guid id)
    {
        var company = await _companyRepository.GetSingleAsync(c => c.Id == id, includes);

        var companyDto = _mapper.Map<CompanyGetResponseDto>(company);
        if (company is null) throw new Exception("Empty");

        return companyDto;
    }

    public async Task<Company> GetCompanyByUsernameAsync(string username)
    {
        var userDetails = await _userService.GetUserByUsernameAsync(username);
        var company = await _companyRepository.GetSingleAsync(c => c.User.UserName == username, includes);

        return company;
    }

    public async Task<ResponseDto> UpdateCompanyAsync(CompanyPutDto companyPutDto)
    {
        var isExist = await _companyRepository.IsExistAsync((b => b.Name.ToLower().Trim() == companyPutDto.Name.ToLower().Trim() && b.Id != companyPutDto.Id));
        if (isExist)
            throw new Exception($"A company with the name '{companyPutDto.Name}' already exists");

        var company = await _companyRepository.GetByIdAsync(companyPutDto.Id);
        if (company is null)
            throw new Exception($"No book found with the ID {companyPutDto.Id}");

        if (companyPutDto.Logo != null)
            FileHelper.DeleteFile(new string[] { _webHostEnvironment.WebRootPath, "assets", "images", "companies", company.Logo });

        if (companyPutDto.BannerImage != null)
            FileHelper.DeleteFile(new string[] { _webHostEnvironment.WebRootPath, "assets", "images", "companies", company.BannerImage });

        Company newCompany = _mapper.Map(companyPutDto, company);

        _companyRepository.Update(newCompany);
        await _companyRepository.SaveAsync();

        return new((int)HttpStatusCode.OK, "The book was successfully updated");
    }

    public async Task<List<CompanyGetResponseDto>> GetCompaniesPendingApprovalOrRejectedAsync()
    {
        var companies = await _companyRepository.GetFiltered(
            c => c.Status == CompanyStatus.PendingApproval || c.Status == CompanyStatus.Rejected).ToListAsync();

        var companyDtos = _mapper.Map<List<CompanyGetResponseDto>>(companies);
        return companyDtos;
    }

    public async Task<ResponseDto> ApproveOrRejectCompanyAsync(Guid companyId, CompanyStatus status)
    {
        var company = await _companyRepository.GetByIdAsync(companyId);

        if (company is null)
            throw new Exception($"No company found with the ID {companyId}");

        if (status != CompanyStatus.Approved && status != CompanyStatus.Rejected)
            throw new ArgumentException("Invalid company status. Only 'Approved' or 'Rejected' are allowed.");

        // Update the company status
        company.Status = status;

        _companyRepository.Update(company);
        await _companyRepository.SaveAsync();

        return new((int)HttpStatusCode.OK, $"The company has been {status.ToString().ToLower()} successfully");
    }

    private static readonly string[] includes ={
        nameof(Company.Books),
        $"{nameof(Company.Books)}.{nameof(Book.Author)}",
        $"{nameof(Company.Books)}.{nameof(Book.Images)}",
        $"{nameof(Company.Books)}.{nameof(Book.Author)}.{nameof(Author.Images)}",
        $"{nameof(Company.Books)}.{nameof(Book.BookGenres)}.{nameof(BookGenre.Genre)}"
    };
}

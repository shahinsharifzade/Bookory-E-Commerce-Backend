using AutoMapper;
using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.BookDtos;
using Bookory.Business.Utilities.DTOs.Common;
using Bookory.Business.Utilities.DTOs.CompanyDtos;
using Bookory.Business.Utilities.DTOs.MailDtos;
using Bookory.Business.Utilities.Email;
using Bookory.Business.Utilities.Enums;
using Bookory.Business.Utilities.Exceptions.BookExceptions;
using Bookory.Business.Utilities.Exceptions.CompanyExceptions;
using Bookory.Business.Utilities.Extension.FileExtensions.Common;
using Bookory.Core.Models;
using Bookory.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using System.Net;

namespace Bookory.Business.Services.Implementations;

public class CompanyService : ICompanyService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ICompanyRepository _companyRepository;
    private readonly IBookService _bookService;
    private readonly IUserService _userService;
    private readonly IMailService _mailService;
    private readonly IMapper _mapper;

    public CompanyService(IMapper mapper, IHttpContextAccessor httpContextAccessor, IUserService userService, ICompanyRepository companyRepository, IWebHostEnvironment webHostEnvironment, IMailService mailService)
    {
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _userService = userService;
        _companyRepository = companyRepository;
        _webHostEnvironment = webHostEnvironment;
        _mailService = mailService;
    }

    public async Task<ResponseDto> CreateCompanyAsync(CompanyPostDto companyPostDto)
    {
        var userDetails = await _userService.GetUserByUsernameAsync(companyPostDto.Username);

        if (userDetails.Role != Roles.Vendor.ToString()) throw new CompanyCreationException("Failed to create a company because the user is not a vendor.");
        if (userDetails.User.IsVendorRegistrationComplete == true)
            throw new CompanyCreationException("Failed to create a company because the user's vendor registration is already complete.");

        var company = _mapper.Map<Company>(companyPostDto);
        company.UserId = userDetails.User.Id;
        company.Status = CompanyStatus.PendingApproval;

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

        var companiesDto = _mapper.Map<List<CompanyGetResponseDto>>(companies);

        if (companies is null || companies.Count == 0) throw new CompanyNotFoundException($"No companies were found based on the search criteria: '{search}' when retrieving approved companies.");

        return companiesDto;
    }

    public async Task<CompanyGetResponseDto> GetCompanyByIdAsync(Guid id)
    {
        var company = await _companyRepository.GetSingleAsync(c => c.Id == id, includes);

        var companyDto = _mapper.Map<CompanyGetResponseDto>(company);
        if (company is null) throw new CompanyNotFoundException($"Company with ID '{id}' was not found.");

        return companyDto;
    }

    public async Task<Company> GetCompanyByUsernameAsync(string username)
    {
        var userDetails = await _userService.GetUserByUsernameAsync(username);
        var company = await _companyRepository.GetSingleAsync(c => c.User.UserName == username, includes);

        if (company is null) throw new CompanyNotFoundException($"No company found for username '{username}'");
        return company;
    }

    public async Task<ResponseDto> UpdateCompanyAsync(CompanyPutDto companyPutDto)
    {
        var isExist = await _companyRepository.IsExistAsync((b => b.Name.ToLower().Trim() == companyPutDto.Name.ToLower().Trim() && b.Id != companyPutDto.Id));
        if (isExist)
            throw new CompanyAlreadyExistException($"Another company with the name '{companyPutDto.Name}' already exists");

        var company = await _companyRepository.GetByIdAsync(companyPutDto.Id);
        if (company is null)
            throw new CompanyNotFoundException($"Unable to update the company. No company was found with the ID {companyPutDto.Id}");

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

        if (companies is null || companies.Count == 0)
            throw new CompanyNotFoundException("No companies pending approval or rejected were found");

            var companyDtos = _mapper.Map<List<CompanyGetResponseDto>>(companies);
        return companyDtos;
    }

    public async Task<ResponseDto> ApproveOrRejectCompanyAsync(Guid companyId, CompanyStatus status)
    {
        var company = await _companyRepository.GetByIdAsync(companyId);

        if (company is null)
            throw new CompanyNotFoundException($"No company was found with the ID {companyId}");

        if (status != CompanyStatus.Approved && status != CompanyStatus.Rejected)
            throw new CompanyStatusException("Invalid company status. Only 'Approved' or 'Rejected' are allowed.");

        company.Status = status;
        _companyRepository.Update(company);
        await _companyRepository.SaveAsync();
        await SendStatusInformationMailAsync(status, company);

        return new((int)HttpStatusCode.OK, $"The company has been {status.ToString().ToLower()} successfully");
    }

    private async Task SendStatusInformationMailAsync(CompanyStatus status, Company? company)
    {
        string emailSubject = $"Company {status}";
        string emailBody = status == CompanyStatus.Approved
        ? "Your company has been approved. Congratulations!"
        : "Your company has been rejected. We apologize for any inconvenience.";

        MailRequestDto mailRequestDto = new(
        company.ContactEmail,
        emailSubject,
        emailBody,
        null);

        await _mailService.SendEmailAsync(mailRequestDto);
    }

    private static readonly string[] includes ={
        nameof(Company.Books),
        $"{nameof(Company.Books)}.{nameof(Book.Author)}",
        $"{nameof(Company.Books)}.{nameof(Book.Images)}",
        $"{nameof(Company.Books)}.{nameof(Book.Author)}.{nameof(Author.Images)}",
        $"{nameof(Company.Books)}.{nameof(Book.BookGenres)}.{nameof(BookGenre.Genre)}"
    };
}

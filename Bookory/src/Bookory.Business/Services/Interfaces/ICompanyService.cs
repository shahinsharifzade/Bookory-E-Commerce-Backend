using Bookory.Business.Utilities.DTOs.BookDtos;
using Bookory.Business.Utilities.DTOs.Common;
using Bookory.Business.Utilities.DTOs.CompanyDtos;
using Bookory.Business.Utilities.Enums;
using Bookory.Core.Models;
using MailKit;

namespace Bookory.Business.Services.Interfaces;

public interface ICompanyService
{
    Task<List<CompanyGetResponseDto>> GetAllCompaniesAsync(string? search);
    Task<CompanyGetResponseDto> GetCompanyByIdAsync(Guid id);
    Task<Company> GetCompanyByUsernameAsync(string username);
    Task<ResponseDto> CreateCompanyAsync(CompanyPostDto companyPostDto);
    Task<ResponseDto> UpdateCompanyAsync(CompanyPutDto companyPutDto);

    Task<CompanyPageResponseDto> GetPageOfCompaniesAsync(int pageNumber, int pageSize, CompanyFiltersDto filters);

    Task<ResponseDto> ApproveOrRejectCompanyAsync(Guid companyId, CompanyStatus status);
    Task<List<CompanyGetResponseDto>> GetCompaniesPendingApprovalOrRejectedAsync();
}


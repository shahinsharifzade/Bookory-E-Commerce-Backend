using Bookory.Business.Utilities.DTOs.Common;
using Bookory.Business.Utilities.DTOs.CompanyDtos;
using Bookory.Business.Utilities.Enums;
using Bookory.Core.Models;

namespace Bookory.Business.Services.Interfaces;

public interface ICompanyService
{
    Task<CompanyPageResponseDto> GetPageOfCompaniesAsync(int pageNumber, int pageSize, CompanyFiltersDto filters);
    Task<List<CompanyGetResponseDto>> GetAllCompaniesAsync(string? search);
    Task<CompanyGetResponseDto> GetCompanyByIdAsync(Guid id);
    Task<Company> GetCompanyByUsernameAsync(string username);
    Task<ResponseDto> CreateCompanyAsync(CompanyPostDto companyPostDto);
    Task<ResponseDto> UpdateCompanyAsync(CompanyPutDto companyPutDto);


    Task<ResponseDto> ApproveOrRejectCompanyAsync(Guid companyId, CompanyStatus status);
    Task<List<CompanyGetResponseDto>> GetCompaniesPendingApprovalOrRejectedAsync();
    Task<ResponseDto> SendMessageAsync(CompanyMessagePostDto messageDto);
    Task ModifyCompanyAsync(Company company);
}


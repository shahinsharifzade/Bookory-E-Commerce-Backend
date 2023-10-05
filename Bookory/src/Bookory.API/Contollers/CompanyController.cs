using Bookory.Business.Services.Implementations;
using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.BookDtos;
using Bookory.Business.Utilities.DTOs.CompanyDtos;
using Bookory.Business.Utilities.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace Bookory.API.Contollers;

[Route("api/[controller]")]
[ApiController]
public class CompanyController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompanyController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(string? search)
    {
        var companies = await _companyService.GetAllCompaniesAsync(search);
        return Ok(companies);
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] CompanyFiltersDto filters)
    {
        return Ok(await _companyService.GetPageOfCompaniesAsync(pageNumber, pageSize, filters));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromForm] CompanyPostDto companyPostDto)
    {
        var response = await _companyService.CreateCompanyAsync(companyPostDto);
        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, [FromForm]CompanyPutDto companyPutDto)
    {
        var dto = new CompanyPutDto(id,companyPutDto.Name, companyPutDto.Description, companyPutDto.Logo, companyPutDto.BannerImage, companyPutDto.ContactPhone, companyPutDto.ContactPhone, companyPutDto.Address);

        var response =await _companyService.UpdateCompanyAsync(dto);
        return Ok(response);
    }

    [HttpGet("pending-or-rejected")]
    public async Task<IActionResult> GetPendingOrRejectedCompanies()
    {
        var companies = await _companyService.GetCompaniesPendingApprovalOrRejectedAsync();
        return Ok(companies);
    }

    [HttpPost("{companyId}/approve")]
    public async Task<IActionResult> ApproveCompany(Guid companyId)
    {
        var response = await _companyService.ApproveOrRejectCompanyAsync(companyId, CompanyStatus.Approved);
        return Ok(response);
    }

    [HttpPost("{companyId}/reject")]
    public async Task<IActionResult> RejectCompany(Guid companyId)
    {
        var response = await _companyService.ApproveOrRejectCompanyAsync(companyId, CompanyStatus.Rejected);
        return Ok(response);
    }

}

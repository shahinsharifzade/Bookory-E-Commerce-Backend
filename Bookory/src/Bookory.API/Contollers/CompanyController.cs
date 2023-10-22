using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.CompanyDtos;
using Bookory.Business.Utilities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return (Ok(await _companyService.GetCompanyByIdAsync(id)));
    }

    [HttpGet("byusername")]
    public async Task<IActionResult> GetByUsername([FromQuery] string username)
    {
        return (Ok(await _companyService.GetCompanyByUsernameAsync(username)));
    }

    [HttpGet("byactive")]
    public async Task<IActionResult> GetByActiveVendor()
    {
        return (Ok(await _companyService.GetCompanyByActiveVendor()));
    }

    [HttpPost]
    [Authorize(Roles = "Admin, Moderator")]
    public async Task<IActionResult> Post([FromForm] CompanyPostDto companyPostDto)
    {
        var response = await _companyService.CreateCompanyAsync(companyPostDto);
        return Ok(response);
    }

    [HttpPost("email")]
    [Authorize(Roles = "Admin, Moderator")]
    public async Task<IActionResult> SendEmail([FromForm] CompanyMessagePostDto companyMessagePostDto)
    {
        return Ok(await _companyService.SendMessageAsync(companyMessagePostDto));
    }

    [HttpPut]
    [Authorize(Roles = "Admin, Moderator")]
    public async Task<IActionResult> Put([FromForm] CompanyPutDto companyPutDto)
    {
        var response = await _companyService.UpdateCompanyAsync(companyPutDto);
        return Ok(response);
    }

    [HttpGet("pending-or-rejected")]
    [Authorize(Roles = "Admin, Moderator")]
    public async Task<IActionResult> GetPendingOrRejectedCompanies()
    {
        var companies = await _companyService.GetCompaniesPendingApprovalOrRejectedAsync();
        return Ok(companies);
    }

    [HttpPost("{companyId}/approve")]
    [Authorize(Roles = "Admin, Moderator")]
    public async Task<IActionResult> ApproveCompany(Guid companyId)
    {
        var response = await _companyService.ApproveOrRejectCompanyAsync(companyId, CompanyStatus.Approved);
        return Ok(response);
    }

    [HttpPost("{companyId}/reject")]
    [Authorize(Roles = "Admin, Moderator")]
    public async Task<IActionResult> RejectCompany(Guid companyId)
    {
        var response = await _companyService.ApproveOrRejectCompanyAsync(companyId, CompanyStatus.Rejected);
        return Ok(response);
    }

}

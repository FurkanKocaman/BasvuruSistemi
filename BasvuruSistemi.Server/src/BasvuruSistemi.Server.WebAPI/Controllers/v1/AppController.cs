using BasvuruSistemi.Server.Application.Applications;
using BasvuruSistemi.Server.Application.FormTemplates;
using BasvuruSistemi.Server.Application.JobPostings;
using BasvuruSistemi.Server.Application.Organizations;
using BasvuruSistemi.Server.Application.Tenants;
using BasvuruSistemi.Server.Application.Users;
using BasvuruSistemi.Server.Domain.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BasvuruSistemi.Server.WebAPI.Controllers.v1;
[Route("v1/api/")]
[ApiController]
public class AppController(ISender sender) : ControllerBase
{
    [HttpGet("user/current")]
    [Authorize]
    public async Task<ActionResult<GetCurrentUserQueryResponse>> GetCurrentUser(CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new GetCurrentUserQuery(), cancellationToken);
        return Ok(response);
    }

    [HttpGet("organizations/authorized")]
    [Authorize]
    public async Task<ActionResult<GetAuthorizedOrganizationsQueryResponse>> GetAuthorizedOrganizations (CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new GetAuthorizedOrganizationsQuery(), cancellationToken);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet("job-postings/active")]
    public async Task<ActionResult<GetActiveJobPostingsQueryResponse>> GetActiveJobPostings(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20, 
    CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new GetActiveJobPostingsQuery(page,pageSize), cancellationToken);
        return Ok(response);
    }

    [HttpGet("job-postings")]
    [Authorize()]
    public async Task<ActionResult<GetJobPostingsByTenantQueryResponse>> GetJobPostings(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20,
    CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new GetJobPostingsByTenantQuery(page, pageSize), cancellationToken);
        return Ok(response);
    }

    [HttpGet("tenants")]
    [Authorize()]
    public async Task<ActionResult<List<GetUserAuthorizedTenantsQueryResponse>>> GetUserAuthorizedTenants(CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new GetUserAuthorizedTenantsQuery(), cancellationToken);
        return Ok(response);
    }

    [HttpGet("form-templates/{id}")]
    [Authorize()]
    public async Task<ActionResult<GetFormTemplateQueryResponse>> GetFormTemplate(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new GetFormTemplateQuery(id), cancellationToken);
        return Ok(response);
    }

    [HttpGet("form-templates")]
    [Authorize()]
    public async Task<IActionResult> GetFormTemplates(
        [FromQuery] string view = "summaries",
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        if(view == "summaries")
        {
            var response = await sender.Send(new GetFormTemplateSummariesQuery(), cancellationToken);
            return Ok(response);
        }
        else if(view == "details")
        {
            var response = await sender.Send(new GetAllFormTemplatesQuery(page, pageSize), cancellationToken);
            return Ok(response);
        }
        return BadRequest();
            
    }

    [HttpGet("organizations")]
    [Authorize()]
    public async Task<ActionResult<GetAllOrganizationsByTenantQueryResponse>> GetAllOrganizationsByTenant(CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new GetAllOrganizationsByTenantQuery(), cancellationToken);
        return Ok(response);
    }

    [HttpGet("applications/{jobPostingId}")]
    [Authorize()]
    public async Task<ActionResult<GetAllApplilactionsQueryResponse>> GetAllApplications(
        Guid jobPostingId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new GetAllApplilactionsQuery(jobPostingId, page, pageSize), cancellationToken);
        return Ok(response);
    }
}

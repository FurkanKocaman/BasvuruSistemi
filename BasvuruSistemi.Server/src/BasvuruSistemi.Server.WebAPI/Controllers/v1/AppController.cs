using BasvuruSistemi.Server.Application.Applications;
using BasvuruSistemi.Server.Application.FormTemplates;
using BasvuruSistemi.Server.Application.JobPostings;
using BasvuruSistemi.Server.Application.PostingGroups;
using BasvuruSistemi.Server.Application.Tenants;
using BasvuruSistemi.Server.Application.Units;
using BasvuruSistemi.Server.Application.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TS.Result;

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

    [HttpGet("units")]
    [Authorize()]
    public async Task<ActionResult<GetAllUnitsByTenantQueryResponse>> GetAllUnitsByTenant(CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new GetAllUnitsByTenantQuery(), cancellationToken);
        return Ok(response);
    }
    [HttpGet("units/{id}")]
    [Authorize()]
    public async Task<ActionResult<GetAllUnitsByTenantQueryResponse>> GetUnit(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new GetUnitQuery(id), cancellationToken);
        return Ok(response);
    }


    [HttpGet("units/authorized")]
    [Authorize]
    public async Task<ActionResult<GetAuthorizedUnitsQueryResponse>> GetAuthorizedOrganizations (CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new GetAuthorizedUnitsQuery(), cancellationToken);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet("job-postings/active")]
    public async Task<ActionResult<GetActiveJobPostingsSummariesQueryResponse>> GetActiveJobPostings(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20, 
    CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new GetActiveJobPostingsSummariesQuery(page,pageSize), cancellationToken);
        return Ok(response);
    }
    [AllowAnonymous]
    [HttpGet("job-postings/active/{id:guid}")]
    public async Task<ActionResult<GetActiveJobPostingQueryResponse>> GetActiveJobPostings(
    Guid? id,
    CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new GetActiveJobPostingQuery(id), cancellationToken);
        return Ok(response);
    }

    [HttpGet("job-postings")]
    [Authorize()]
    public async Task<IActionResult> GetJobPostings(
    [FromQuery] string view = "summaries",
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20,
    CancellationToken cancellationToken = default)
    {
        if(view == "summaries")
        {
            var response = await sender.Send(new GetJobPostingsSummariesByTenantQuery(page, pageSize), cancellationToken);
            return Ok(response);
        }
        else
        {
            var response = await sender.Send(new GetJobPostingsByTenantQuery(page, pageSize), cancellationToken);
            return Ok(response);
        }
           
    }


    [HttpGet("job-postings/{id:guid}")]
    [Authorize()]
    public async Task<ActionResult<GetJobPostingsQueryResponse>> GetJobPostings(
    Guid id,
    CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new GetJobPostingsQuery(id), cancellationToken);
        return response.IsSuccessful ? Ok(response) : BadRequest(response);
    }

    [AllowAnonymous]
    [HttpGet("posting-groups/{id:guid}")]
    public async Task<ActionResult<PostingGroupGetQueryResponse>> GetPostingGroup(
    Guid id,
    CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new PostingGroupGetQuery(id), cancellationToken);
        return Ok(response);
    }

    [HttpGet("posting-groups")]
    [Authorize()]
    public async Task<ActionResult<GetAllPostingGroupSummariesByTenantQueryResponse>> GetPostingGroupSummaries(
    CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new GetAllPostingGroupSummariesByTenantQuery(), cancellationToken);
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

  
    [HttpGet("applications/{jobPostingId?}")]
    [Authorize()]
    public async Task<ActionResult<GetAllApplilactionsQueryResponse>> GetAllApplications(
        Guid? jobPostingId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new GetAllApplilactionsQuery(jobPostingId, page, pageSize), cancellationToken);
        return Ok(response);
    }
    [HttpGet("applications/detail/{applicationId}")]
    [Authorize()]
    public async Task<ActionResult<GetAllApplilactionsQueryResponse>> GetApplication(
    Guid applicationId,
    CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new GetApplicationQuery(applicationId), cancellationToken);
        return Ok(response);
    }

    [HttpGet("applications/check-existing/{jobPostingId}")]
    [Authorize()]
    public async Task<ActionResult<Result<bool>>> CheckApplicationExist(
    Guid jobPostingId,
    CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new CheckApplicationExistsQuery(jobPostingId), cancellationToken);
        return Ok(response);
    }

    [HttpGet("applications-by-user")]
    [Authorize()]
    public async Task<ActionResult<GetApplicationsByUserQueryResponse>> GetApplicationByUser(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20,
    CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new GetApplicationsByUserQuery(page,pageSize), cancellationToken);
        return Ok(response);
    }
}

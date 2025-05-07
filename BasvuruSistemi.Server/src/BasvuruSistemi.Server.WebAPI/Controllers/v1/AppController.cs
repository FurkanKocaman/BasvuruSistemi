using BasvuruSistemi.Server.Application.JobPostings;
using BasvuruSistemi.Server.Application.Organizations;
using BasvuruSistemi.Server.Application.Users;
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

    [HttpGet("job-postings")]

    public async Task<ActionResult<GetActiveJobPostingsQueryResponse>> GetActiveJobPostings(
    [FromQuery] string view = "summaries",
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20, 
    CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new GetActiveJobPostingsQuery(page,pageSize), cancellationToken);
        return Ok(response);
    }
}

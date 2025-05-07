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
}

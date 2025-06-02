using BasvuruSistemi.Server.Application.ApplicationEvaluations;
using BasvuruSistemi.Server.Application.Applications;
using BasvuruSistemi.Server.Application.Comissions;
using BasvuruSistemi.Server.Application.EvaluationForms;
using BasvuruSistemi.Server.Application.Evaluations;
using BasvuruSistemi.Server.Application.FormTemplates;
using BasvuruSistemi.Server.Application.JobPostings;
using BasvuruSistemi.Server.Application.PostingGroups;
using BasvuruSistemi.Server.Application.RoleAssignments;
using BasvuruSistemi.Server.Application.Tenants;
using BasvuruSistemi.Server.Application.Units;
using BasvuruSistemi.Server.Application.Users;
using BasvuruSistemi.Server.Domain.DTOs;
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
    [Authorize()]
    public async Task<ActionResult<GetCurrentUserQueryResponse>> GetCurrentUser(CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new GetCurrentUserQuery(), cancellationToken);
        return StatusCode(response.StatusCode,response.StatusCode == 200 ? response.Data : response.ErrorMessages);
    }
    [HttpGet("users")]
    [Authorize()]
    public async Task<ActionResult<PagedResult<GetAllUsersQueryResponse>>> GetAllUsers(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new GetAllUsersQuery(page,pageSize), cancellationToken);
        return StatusCode(response.StatusCode, response.StatusCode == 200 ? response.Data : response.ErrorMessages);
    }

    [HttpGet("users/profile")]
    [Authorize]
    public async Task<ActionResult<GetUserProfileQueryResponse>> GetUserProfile([FromQuery]Guid? id, CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new GetUserProfileQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response.StatusCode == 200 ? response.Data : response.ErrorMessages);
    }

    [HttpGet("roles")]
    [Authorize]
    public async Task<ActionResult> GetRoles(
        [FromQuery] string view = "summaries",
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        if(view == "summaries")
        {
            var response = await sender.Send(new GetAllRolesSummariesByTenantQuery(), cancellationToken);
            return StatusCode(response.StatusCode, response.StatusCode == 200 ? response.Data : response.ErrorMessages);
        }
        else
        {
            var response = await sender.Send(new GetAllRolesDetailsByTenantQuery(page, pageSize), cancellationToken);
            return StatusCode(response.StatusCode, response.StatusCode == 200 ? response.Data : response.ErrorMessages);
        }
    }

    [HttpGet("token/{token}")]
    [Authorize]
    public async Task<ActionResult<Result<GetInformationByTokenQueryResponse>>> GetTokenInformation(string token, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new GetInformationByTokenQuery(token), cancellationToken);
        return StatusCode(response.StatusCode, response.StatusCode == 200 ? response.Data : response.ErrorMessages);
    }

    [HttpGet("units")]
    [Authorize()]
    public async Task<ActionResult<GetAllUnitsByTenantQueryResponse>> GetAllUnitsByTenant(CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new GetAllUnitsByTenantQuery(), cancellationToken);
        return StatusCode(response.StatusCode, response.StatusCode == 200 ? response.Data : response.ErrorMessages);
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
        return StatusCode(response.StatusCode, response.StatusCode == 200 ? response.Data : response.ErrorMessages);
    }

    [AllowAnonymous]
    [HttpGet("job-postings/active")]
    public async Task<ActionResult<GetActiveJobPostingsSummariesQueryResponse>> GetActiveJobPostings(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20, 
    CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new GetActiveJobPostingsSummariesQuery(page,pageSize), cancellationToken);
        return StatusCode(response.StatusCode, response.StatusCode == 200 ? response.Data : response.ErrorMessages);
    }

    [AllowAnonymous]
    [HttpGet("job-postings/active/{id:guid}")]
    public async Task<ActionResult<GetActiveJobPostingQueryResponse>> GetActiveJobPostings(
    Guid? id,
    CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new GetActiveJobPostingQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response.StatusCode == 200 ? response.Data : response.ErrorMessages);
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
            return StatusCode(response.StatusCode, response.StatusCode == 200 ? response.Data : response.ErrorMessages);
        }
        else
        {
            var response = await sender.Send(new GetJobPostingsByTenantQuery(page, pageSize), cancellationToken);
            return StatusCode(response.StatusCode, response.StatusCode == 200 ? response.Data : response.ErrorMessages);
        }
    }

    [HttpGet("job-postings/{id:guid}")]
    [Authorize()]
    public async Task<ActionResult<GetJobPostingsQueryResponse>> GetJobPostings(
    Guid id,
    CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new GetJobPostingsQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response.StatusCode == 200 ? response.Data : response.ErrorMessages);
    }

    [AllowAnonymous]
    [HttpGet("posting-groups/{id:guid}")]
    public async Task<ActionResult<PostingGroupGetQueryResponse>> GetPostingGroup(
    Guid id,
    CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new PostingGroupGetQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response.StatusCode == 200 ? response.Data : response.ErrorMessages);
    }

    [HttpGet("posting-groups")]
    [Authorize()]
    public async Task<ActionResult<GetAllPostingGroupSummariesByTenantQueryResponse>> GetPostingGroupSummaries(
    CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new GetAllPostingGroupSummariesByTenantQuery(), cancellationToken);
        return StatusCode(response.StatusCode, response.StatusCode == 200 ? response.Data : response.ErrorMessages);
    }

    [HttpGet("tenants")]
    [Authorize()]
    public async Task<ActionResult<List<GetUserAuthorizedTenantsQueryResponse>>> GetUserAuthorizedTenants(CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new GetUserAuthorizedTenantsQuery(), cancellationToken);
        return StatusCode(response.StatusCode, response.StatusCode == 200 ? response.Data : response.ErrorMessages);
    }

    [HttpGet("form-templates/{id}")]
    [Authorize()]
    public async Task<ActionResult<GetFormTemplateQueryResponse>> GetFormTemplate(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new GetFormTemplateQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response.StatusCode == 200 ? response.Data : response.ErrorMessages);
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
            return StatusCode(response.StatusCode, response.StatusCode == 200 ? response.Data : response.ErrorMessages);
        }
        else if(view == "details")
        {
            var response = await sender.Send(new GetAllFormTemplatesQuery(page, pageSize), cancellationToken);
            return StatusCode(response.StatusCode, response.StatusCode == 200 ? response.Data : response.ErrorMessages);
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
        return StatusCode(response.StatusCode, response.StatusCode == 200 ? response.Data : response.ErrorMessages);
    }


    [HttpGet("applications/detail/{applicationId}")]
    [Authorize()]
    public async Task<ActionResult<GetAllApplilactionsQueryResponse>> GetApplication(
    Guid applicationId,
    CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new GetApplicationQuery(applicationId), cancellationToken);
        return StatusCode(response.StatusCode, response.StatusCode == 200 ? response.Data : response.ErrorMessages);
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

    [HttpGet("applications/{id}/values")]
    [Authorize()]
    public async Task<ActionResult<GetApplicationForUpdateQueryResponse>> GetApplicationForUpdate(
    Guid id,
    CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new GetApplicationForUpdateQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response.Data);
    }

    [HttpGet("applications-by-user")]
    [Authorize()]
    public async Task<ActionResult<GetApplicationsByUserQueryResponse>> GetApplicationByUser(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20,
    CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new GetApplicationsByUserQuery(page,pageSize), cancellationToken);
        return StatusCode(response.StatusCode, response.StatusCode == 200 ? response.Data : response.ErrorMessages);
    }

    [HttpGet("commissions/{id:guid}")]
    [Authorize()]
    public async Task<ActionResult<GetApprovalCommissionByIdQueryResponse>> GetApprovalCommission(
        Guid id,
       CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new GetApprovalCommissionByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response.StatusCode == 200 ? response.Data : response.ErrorMessages);
    }

    [HttpGet("commissions")]
    [Authorize()]
    public async Task<ActionResult<List<GetApprovalCommissionByIdQueryResponse>>> ListApprovalCommissions(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new ListApprovalCommissionsQuery(), cancellationToken);
        return StatusCode(response.StatusCode, response.StatusCode == 200 ? response.Data : response.ErrorMessages);
    }

    [HttpGet("evaluation-forms/{id:guid}")]
    [Authorize()]
    public async Task<ActionResult<EvaluationFormDto>> GetEvaluationFormById(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new GetEvaluationFormByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response.StatusCode == 200 ? response.Data : response.ErrorMessages);
    }

    [HttpGet("evaluation-stages")]
    [Authorize()]
    public async Task<ActionResult<EvaluationStageDto>> ListEvaluationStages(
      Guid id,
      CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new ListEvaluationStagesQuery(), cancellationToken);
        return StatusCode(response.StatusCode, response.StatusCode == 200 ? response.Data : response.ErrorMessages);
    }

    [HttpGet("evaluation-stage/{id:guid}")]
    [Authorize()]
    public async Task<ActionResult<EvaluationStageDto>> GetEvaluationStageById(
      Guid id,
      CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new GetEvaluationStageByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response.StatusCode == 200 ? response.Data : response.ErrorMessages);
    }

    [HttpGet("evaluation-stage/forms/{id:guid}")]
    [Authorize()]
    public async Task<ActionResult<EvaluationFormDto>> GetEvaluationFormByStageId(
       Guid id,
       CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new ListEvaluationFormsByStageQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response.StatusCode == 200 ? response.Data : response.ErrorMessages);
    }

    [HttpGet("list-pending-evaluations")]
    [Authorize()]
    public async Task<ActionResult<ListPendingCommissionEvaluationsQueryResponse>> GetPendingApplicationEvaluations(
        [FromQuery] Guid? jobPostingId,
        [FromQuery] Guid? evaluationStageId,
        CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new ListPendingCommissionEvaluationsQuery(jobPostingId,evaluationStageId),cancellationToken);
        return StatusCode(response.StatusCode, response.Data);
    }

    [HttpGet("evaluation-form")]
    [Authorize()]
    public async Task<ActionResult<GetEvaluationFormForApplicationEvaluationQueryResponse>> GetEvaliationForm(
        [FromQuery] Guid applicationEvaluationId,
        CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new GetEvaluationFormForApplicationEvaluationQuery(applicationEvaluationId), cancellationToken);
        return StatusCode(response.StatusCode, response.Data);
    }

    [HttpGet("application-evaluations/process")]
    [Authorize()]
    public async Task<ActionResult<List<GetApplicationEvaluationProcessQueryResponse>>> getApplicationEvaluationProcess(
    [FromQuery] Guid applicationId,
    CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new GetApplicationEvaluationProcessQuery(applicationId), cancellationToken);
        return StatusCode(response.StatusCode, response.Data);
    }
}

using BasvuruSistemi.Server.Domain.ApplicationEvaluations;
using BasvuruSistemi.Server.Domain.Applications;
using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Applications;
public sealed record GetApplicationQuery(
    Guid applicationId
    ) : IRequest<Result<GetApplicationQueryResponse>>;

public sealed class GetApplicationQueryResponse
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? Email { get; set; } 
    public string? Phone { get; set; } 
    public string? TCKN { get; set; }

    public Guid JobPostingId { get; set; }
    public string JobPostingTitle { get; set; } = default!;
    public DateTimeOffset JobPostingCreateDate { get; set; } = default!;
    public int? JobPostingVacancyCount { get; set; }
    public int? TotalApplicationCount { get; set; }
    public string Unit { get; set; } = default!;

    public DateTimeOffset AppliedDate { get; set; }
    public ApplicationStatus Status { get; set; } = default!;
    public DateTimeOffset? ReviewDate { get; set; }
    public string? ReviewDescription { get; set; }


    public List<FieldValueResponseDto> FieldValues { get; set; } = new List<FieldValueResponseDto>();
}

internal sealed class GetApplicationQueryHandler(
    IApplicationRepository applicationRepository,
    IApplicationEvaluationRepository applicationEvaluationRepository,
    IHttpContextAccessor httpContextAccessor
    ) : IRequestHandler<GetApplicationQuery, Result<GetApplicationQueryResponse>>
{
    public async Task<Result<GetApplicationQueryResponse>> Handle(
        GetApplicationQuery request,
        CancellationToken cancellationToken)
    {
        var applicationEvaluation = await applicationEvaluationRepository.Where(p => p.Id == request.applicationId).FirstOrDefaultAsync(cancellationToken);

        var application = await applicationRepository
            .Where(p => p.Id == request.applicationId ||(applicationEvaluation != null ? p.Id == applicationEvaluation.ApplicationId : false))
            .Include(p => p.User)
                .ThenInclude(u => u.Addresses)
            .Include(p => p.FieldValues)
                .ThenInclude(fv => fv.FieldDefinition)
            .Include(p => p.JobPosting)
                .ThenInclude(jp => jp.Tenant)
            .Include(p => p.JobPosting)
                .ThenInclude(jp => jp.Unit)
            .FirstOrDefaultAsync(cancellationToken);

        if (application is null)
            return Result<GetApplicationQueryResponse>.Failure("Application not found.");

        var totalApplicationCount = await applicationRepository
            .Where(p => p.JobPostingId == application.JobPostingId && !p.IsDeleted)
            .CountAsync(cancellationToken);

        var context = httpContextAccessor.HttpContext?.Request;
        string baseUrl = context is null
            ? string.Empty
            : $"{context.Scheme}://{context.Host}";

        var response = new GetApplicationQueryResponse
        {
            Id = application.Id,
            UserId = application.UserId,
            FirstName = application.User.FirstName,
            LastName = application.User.LastName,
            Email = application.User.Email,
            Phone = application.User.Contact.Phone,
            TCKN = application.User.TCKN,

            JobPostingId = application.JobPostingId,
            JobPostingTitle = application.JobPosting.Title,
            JobPostingCreateDate = application.JobPosting.CreatedAt,
            JobPostingVacancyCount = application.JobPosting.VacancyCount,
            TotalApplicationCount = totalApplicationCount,
            Unit = application.JobPosting.Unit != null
                ? application.JobPosting.Unit.Name
                : application.JobPosting.Tenant.Name,

            AppliedDate = application.AppliedDate,
            Status = application.Status,
            ReviewDate = application.ReviewDate,
            ReviewDescription = application.ReviewDescription,

            FieldValues = application.FieldValues
                .OrderBy(fv => fv.FieldDefinition.Order)
                .Select(fv =>
                {
                    var value = fv.Value;

                    if (fv.FieldDefinition.Type is
                        FieldTypeEnum.File or
                        FieldTypeEnum.Image or
                        FieldTypeEnum.EDevletVerifiedFile or
                        FieldTypeEnum.YoksisAlesDocument)
                    {
                        value = $"{baseUrl}{fv.Value}";
                    }
                    return new FieldValueResponseDto(
                        fv.FieldDefinitionId,
                        fv.FieldDefinition.Label,
                        fv.FieldDefinition.Type,
                        value
                    );
                })
                .ToList()
        };

        return Result<GetApplicationQueryResponse>.Succeed(response);
    }
}


using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.JobPostings;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.JobPostings;

public sealed record JobPostingUpdateCommand(
    Guid Id,

    string Title,
    string Description,
    string? Responsibilities,
    string? Qualifications,
    string? Benefits,

    DateTimeOffset datePosted,
    DateTimeOffset ApplicationDeadline,
    DateTimeOffset? ValidFrom,
    DateTimeOffset? ValidTo,

    bool IsRemote,
    string? LocationText,

    int? VacancyCount,
    int? EmploymentType, //EmploymentType Enum
    int? ExperienceLevelRequired, //ExperienceLevel Enum
    string? SkillsRequired,

    List<string>? AllowedNationalIds,

    string? ContactInfo,
    bool IsPublic,
    bool IsAnonymous,

    decimal? MinSalary,
    decimal? MaxSalary,
    string? Currency,

    Guid? UnitId,

    Guid FormTemplateId,

    Guid? PostingGroupId
) : IRequest<Result<string>>;

internal sealed class JobPostingUpdateCommandHandler(
    IJobPostingRepository jobPostingRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService
) : IRequestHandler<JobPostingUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(JobPostingUpdateCommand request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;

        if (!tenantId.HasValue)
            return Result<string>.Failure("Tenant not found");

        var jobPosting = await jobPostingRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if (jobPosting is null)
            return Result<string>.Failure("Job posting nor found");

        if (request.EmploymentType is not null && !System.Enum.IsDefined(typeof(EmploymentType), request.EmploymentType))
        {
            return Result<string>.Failure($"Invalid EmploymentType: {request.EmploymentType}.");
        }
        EmploymentType? employmentType = (EmploymentType?)request.EmploymentType;

        if (request.ExperienceLevelRequired is not null && !System.Enum.IsDefined(typeof(ExperienceLevel), request.ExperienceLevelRequired))
        {
            return Result<string>.Failure($"Invalid ExperienceLevelRequired: {request.ExperienceLevelRequired}.");
        }
        ExperienceLevel? experienceLevelRequired = (ExperienceLevel?)request.ExperienceLevelRequired;

        jobPosting.UpdateDetails(
            request.Title,
            request.Description,
            request.ApplicationDeadline,
            tenantId.Value,
            request.UnitId,
            request.FormTemplateId,
            request.PostingGroupId,
            request.IsPublic,
            request.IsAnonymous,

            request.ValidFrom,
            request.ValidTo,
            
            request.ContactInfo,

            request.Responsibilities,
            request.Qualifications,
            request.Benefits,
            request.LocationText,
            request.IsRemote,
            employmentType,
            experienceLevelRequired,
            request.VacancyCount,
            request.SkillsRequired,

            request.MinSalary,
            request.MaxSalary,
            request.Currency
        );

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("JobPosting saved successfully");
    }
}

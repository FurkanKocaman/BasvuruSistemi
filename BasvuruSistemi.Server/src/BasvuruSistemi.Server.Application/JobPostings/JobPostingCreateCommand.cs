using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.JobPostings;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.JobPostings;
public sealed record JobPostingCreateCommand(
    string title,
    string description,
    string? responsibilities,
    string? qualifications,
    string? benefits,

    DateTimeOffset datePosted,
    DateTimeOffset applicationDeadline,
    DateTimeOffset? validFrom,
    DateTimeOffset? validTo,

    int status, //JobPostingStatus Enum
    bool isRemote,
    string? locationText,

    int? vacancyCount,
    int? employementType, //EmploymentType Enum
    int? experienceLevelRequired, //ExperienceLevel Enum
    string? skillsRequired,

    List<string>? allowedNationalIds,

    string? contactInfo,
    bool isPublic,
    bool isAnonymous,
    decimal? minSalary,
    decimal? maxSalary,
    string? currency,

    Guid? unitId,

    Guid formTemplateId,

    Guid? postingGroupId
    ) : IRequest<Result<string>>;


internal sealed class JobPostingCreateCommandHandler(
    ICurrentUserService currentUserService,
    IJobPostingRepository jobPostingRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<JobPostingCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(JobPostingCreateCommand request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;

        if (!tenantId.HasValue)
            return Result<string>.Failure("Tenant not found");

        if (!System.Enum.IsDefined(typeof(JobPostingStatus), request.status))
        {
            return Result<string>.Failure($"Invalid JobPostingStatus: {request.status}.");
        }
        JobPostingStatus status = (JobPostingStatus)request.status;

        if (request.employementType is not null &&  !System.Enum.IsDefined(typeof(EmploymentType), request.employementType))
        {
            return Result<string>.Failure($"Invalid JobPostingStatus: {request.status}.");
        }
        EmploymentType? employementType = (EmploymentType?)request.employementType ;

        if (request.experienceLevelRequired is not null && !System.Enum.IsDefined(typeof(ExperienceLevel), request.experienceLevelRequired))
        {
            return Result<string>.Failure($"Invalid JobPostingStatus: {request.experienceLevelRequired}.");
        }
        ExperienceLevel? experienceLevelRequired = (ExperienceLevel?)request.experienceLevelRequired;

        JobPosting jobPosting = new(
             request.title
            ,request.description
            ,request.datePosted
            ,request.applicationDeadline
            ,tenantId.Value
            ,request.unitId
            ,request.formTemplateId
            ,request.postingGroupId
            ,status
            ,request.isPublic
            ,request.isAnonymous

            , request.validFrom
            ,request.validTo

            ,request.contactInfo

            ,request.responsibilities
            ,request.qualifications
            ,request.benefits
            ,request.locationText
            ,request.isRemote
            ,employementType
            ,experienceLevelRequired
            ,request.vacancyCount
            ,request.skillsRequired

            ,request.minSalary
            ,request.maxSalary
            ,request.currency
            );

        if(request.allowedNationalIds is not null)
        {
            jobPosting.SetAllowedNationalIds(request.allowedNationalIds);
        }

        jobPostingRepository.Add(jobPosting);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Job posting created");
    }
}

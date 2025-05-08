using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.JobPostings;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.JobPostings;

public sealed record JobPostingUpdateCommand(
    Guid id,
    string title,
    string description,
    string? responsibilities,
    string? qualifications,
    string? benefits,

    DateTimeOffset datePosted,
    DateTimeOffset applicationDeadLine,
    DateTimeOffset? validFrom,
    DateTimeOffset? validTo,

    bool isRemote,
    string? locationText,

    int? vacancyCount,
    int? employmentType, // EmploymentType Enum
    int? experienceLevelRequired, // ExperienceLevel Enum
    string? salaryRange,
    string? skillsRequired,

    string? contactInfo,
    bool isPublic,

    Guid? postingGroupId
) : IRequest<Result<string>>;

internal sealed class JobPostingUpdateCommandHandler(
    IJobPostingRepository jobPostingRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<JobPostingUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(JobPostingUpdateCommand request, CancellationToken cancellationToken)
    {
        var jobPosting = await jobPostingRepository.FirstOrDefaultAsync(p => p.Id == request.id, cancellationToken);
        if (jobPosting is null)
            return Result<string>.Failure("Job posting nor found");

        jobPosting.UpdateDetails(
            request.title,
            request.description,
            request.responsibilities,
            request.qualifications,
            request.applicationDeadLine,
            request.locationText,
            request.isRemote,
            request.employmentType.HasValue ? (EmploymentType)request.employmentType.Value : null,
            request.experienceLevelRequired.HasValue ? (ExperienceLevel)request.experienceLevelRequired.Value : null,
            request.vacancyCount
        );

        typeof(JobPosting).GetProperty("DatePosted")?.SetValue(jobPosting, request.datePosted);
        typeof(JobPosting).GetProperty("ValidFrom")?.SetValue(jobPosting, request.validFrom);
        typeof(JobPosting).GetProperty("ValidTo")?.SetValue(jobPosting, request.validTo);
        typeof(JobPosting).GetProperty("SalaryRange")?.SetValue(jobPosting, request.salaryRange);
        typeof(JobPosting).GetProperty("SkillsRequired")?.SetValue(jobPosting, request.skillsRequired);
        typeof(JobPosting).GetProperty("ContactInfo")?.SetValue(jobPosting, request.contactInfo);
        typeof(JobPosting).GetProperty("IsPublic")?.SetValue(jobPosting, request.isPublic);
        typeof(JobPosting).GetProperty("PostingGroupId")?.SetValue(jobPosting, request.postingGroupId);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Post saved successfully");
    }
}

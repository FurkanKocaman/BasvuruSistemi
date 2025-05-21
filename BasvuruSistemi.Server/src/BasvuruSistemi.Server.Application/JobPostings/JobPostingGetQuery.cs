using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.JobPostings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.JobPostings;
public sealed record GetJobPostingsQuery(
    Guid id
    ) : IRequest<Result<GetJobPostingsQueryResponse>>;

public sealed class GetJobPostingsQueryResponse
{
    public Guid Id { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string? Responsibilities { get; set; }
    public string? Qualifications { get; set; }
    public string? Benefits { get; set; }

    public DateTimeOffset DatePosted { get; set; }
    public DateTimeOffset ApplicationDeadline { get; set; }
    public DateTimeOffset? ValidFrom { get; set; }
    public DateTimeOffset? ValidTo { get; set; }

    public JobPostingStatus Status { get; set; }
    public bool IsRemote { get; set; }
    public string? LocationText { get; set; }

    public int? VacancyCount { get; set; }
    public string? EmploymentType { get; set; }
    public string? ExperienceLevelRequired { get; set; }
    public string? SkillsRequired { get; set; }

    public List<string>? AllowedNationalIds { get; set; }

    public string? ContactInfo { get; set; }
    public bool IsPublic { get; set; }
    public bool IsAnonymous { get; set; }
    public decimal? MinSalary { get; set; }
    public decimal? MaxSalary { get; set; }
    public string? Curreny { get; set; }

    public Guid? UnitId { get; set; }

    public Guid FormTemplateId { get; set; }

    public Guid? PostingGroupId { get; set; }

    public string Tenant { get; set; } = default!;
    public string? Unit { get; set; }
}

internal sealed class GetJobPostingsQueryHandler(
    IJobPostingRepository jobPostingRepository
    ) : IRequestHandler<GetJobPostingsQuery, Result<GetJobPostingsQueryResponse>>
{
    public Task<Result<GetJobPostingsQueryResponse>> Handle(GetJobPostingsQuery request, CancellationToken cancellationToken)
    {
        var jobPosting = jobPostingRepository.Where(p => p.Id == request.id).Include(p => p.Tenant).Include(p => p.Unit).FirstOrDefault();

        if (jobPosting is null)
            return Task.FromResult(Result<GetJobPostingsQueryResponse>.Failure("Job posting not found"));

        var response = new GetJobPostingsQueryResponse()
        {
            Id = jobPosting.Id,

            Title = jobPosting.Title,
            Description = jobPosting.Description,
            Responsibilities = jobPosting.Responsibilities,
            Qualifications = jobPosting.Qualifications,
            Benefits = jobPosting.Benefits,

            DatePosted = jobPosting.DatePosted,
            ApplicationDeadline = jobPosting.ApplicationDeadline,
            ValidFrom = jobPosting.ValidFrom,
            ValidTo = jobPosting.ValidTo,

            Status = jobPosting.Status,
            IsRemote = jobPosting.IsRemote,
            LocationText = jobPosting.LocationText,

            VacancyCount = jobPosting.VacancyCount,
            EmploymentType = jobPosting.EmploymentType.ToString(),
            ExperienceLevelRequired = jobPosting.ExperienceLevelRequired.ToString(),
            SkillsRequired = jobPosting.SkillsRequired,
            AllowedNationalIds = jobPosting.AllowedNationalIds,

            ContactInfo = jobPosting.ContactInfo,
            IsPublic = jobPosting.IsPublic,
            IsAnonymous = jobPosting.IsAnonymous,
            MinSalary = jobPosting.MinSalary,
            MaxSalary = jobPosting.MaxSalary,
            Curreny = jobPosting.Currency,

            UnitId = jobPosting.UnitId,

            FormTemplateId = jobPosting.FormTemplateId,

            PostingGroupId = jobPosting.PostingGroupId,

            Tenant = jobPosting.Tenant.Name,
            Unit = jobPosting.Unit != null ? jobPosting.Unit.Name : null,
        };

        return Task.FromResult(Result<GetJobPostingsQueryResponse>.Succeed(response));
    }
}

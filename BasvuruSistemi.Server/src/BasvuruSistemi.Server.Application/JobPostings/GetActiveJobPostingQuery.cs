using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.JobPostings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.JobPostings;

public sealed record GetActiveJobPostingQuery(
    Guid? id
) : IRequest<Result<GetActiveJobPostingQueryResponse>>;

public sealed class GetActiveJobPostingQueryResponse
{
    public Guid Id { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string? Responsibilities { get; set; }
    public string? Qualifications { get; set; }
    public string? Benefits { get; set; }

    public DateTimeOffset? ValidFrom { get; set; }
    public DateTimeOffset? ValidTo { get; set; }

    public bool IsRemote { get; set; }
    public string? LocationText { get; set; }

    public int? VacancyCount { get; set; }
    public string? EmploymentType { get; set; }
    public string? ExperienceLevelRequired { get; set; }
    public string? SalaryRange { get; set; }
    public string? SkillsRequired { get; set; }

    public string? ContactInfo { get; set; }
    public bool IsPublic { get; set; }

    public string Tenant { get; set; } = default!;
    public string? Unit { get; set; }

    public Guid FormTemplateId { get; set; }
}

internal sealed class GetActiveJobPostingQueryHandler(
    IJobPostingRepository jobPostingRepository
) : IRequestHandler<GetActiveJobPostingQuery, Result<GetActiveJobPostingQueryResponse>>
{
    public async Task<Result<GetActiveJobPostingQueryResponse>> Handle(GetActiveJobPostingQuery request, CancellationToken cancellationToken)
    {
        var query = jobPostingRepository
            .Where(p => p.Status == JobPostingStatus.Published && p.IsPublic && !p.IsDeleted);

        if (request.id.HasValue)
            query = query.Where(p => p.Id == request.id.Value);

        var jobPosting = await query
            .Include(p => p.Tenant)
            .Include(p => p.Unit)
            .FirstOrDefaultAsync(cancellationToken);

        if (jobPosting is null)
            return Result<GetActiveJobPostingQueryResponse>.Failure("İlan bulunamadı.");

        var response = new GetActiveJobPostingQueryResponse
        {
            Id = jobPosting.Id,
            Title = jobPosting.Title,
            Description = jobPosting.Description,
            Responsibilities = jobPosting.Responsibilities,
            Qualifications = jobPosting.Qualifications,
            Benefits = jobPosting.Benefits,

            ValidFrom = jobPosting.ValidFrom,
            ValidTo = jobPosting.ValidTo,

            IsRemote = jobPosting.IsRemote,
            LocationText = jobPosting.LocationText,

            VacancyCount = jobPosting.VacancyCount,
            EmploymentType = jobPosting.EmploymentType?.ToString(),
            ExperienceLevelRequired = jobPosting.ExperienceLevelRequired?.ToString(),
            SalaryRange = $"{jobPosting.MinSalary} {jobPosting.Currency} - {jobPosting.MaxSalary} {jobPosting.Currency}",
            SkillsRequired = jobPosting.SkillsRequired,

            ContactInfo = jobPosting.ContactInfo,
            IsPublic = jobPosting.IsPublic,

            Tenant = jobPosting.Tenant.Name,
            Unit = jobPosting.Unit?.Name,

            FormTemplateId = jobPosting.FormTemplateId,
        };

        return Result<GetActiveJobPostingQueryResponse>.Succeed(response);
    }
}

using BasvuruSistemi.Server.Domain.Companies;
using BasvuruSistemi.Server.Domain.Departments;
using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.JobPostings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BasvuruSistemi.Server.Application.JobPostings;
public sealed record GetActiveJobPostingsQuery(
    int page,
    int pageSize
    ) : IRequest<PagedResult<GetActiveJobPostingsQueryResponse>>;

public sealed class GetActiveJobPostingsQueryResponse
{
    public Guid Id { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string? Responsibilities { get;  set; }
    public string? Qualifications { get;  set; }
    public string? Benefits { get;  set; }

    public DateTimeOffset? ValidFrom { get;  set; }
    public DateTimeOffset? ValidTo { get;  set; }

    public bool IsRemote { get;  set; }
    public string? LocationText { get;  set; }

    public int? VacancyCount { get; set; }                // Açık pozisyon sayısı (Kontenjan)
    public string? EmploymentType { get; set; }
    public string? ExperienceLevelRequired { get; set; }
    public string? SalaryRange { get; set; }
    public string? SkillsRequired { get; set; }

    public string? ContactInfo { get; set; }
    public bool IsPublic { get; set; }

    public string Company { get; set; } = default!;
    public string? Department { get; set; }

}

internal sealed class GetActiveJobPostingsQueryHandler(
    IJobPostingRepository jobPostingRepository
    ) : IRequestHandler<GetActiveJobPostingsQuery, PagedResult<GetActiveJobPostingsQueryResponse>>
{
    public Task<PagedResult<GetActiveJobPostingsQueryResponse>> Handle(GetActiveJobPostingsQuery request, CancellationToken cancellationToken)
    {
        var jobPostings = jobPostingRepository
            .Where(p => p.Status == JobPostingStatus.Published && p.IsPublic && !p.IsDeleted).Include(p => p.Company).Include(p => p.Department);

        var totalCount = jobPostings.Count(); 

        var pagedJobPostings = jobPostings
            .Skip((request.page - 1) * request.pageSize)
            .Take(request.pageSize)
            .ToList();

        var response = jobPostings.Select(p => new GetActiveJobPostingsQueryResponse
        {
            Id = p.Id,

            Title = p.Title,
            Description = p.Description,
            Responsibilities = p.Responsibilities,
            Qualifications = p.Qualifications,
            Benefits = p.Benefits,

            ValidFrom = p.ValidFrom,
            ValidTo = p.ValidTo,

            IsRemote = p.IsRemote,
            LocationText = p.LocationText,

            VacancyCount = p.VacancyCount,
            EmploymentType = p.EmploymentType.ToString(),
            ExperienceLevelRequired = p.ExperienceLevelRequired.ToString(),
            SalaryRange = p.SalaryRange,
            SkillsRequired = p.SkillsRequired,

            ContactInfo = p.ContactInfo,
            IsPublic = p.IsPublic,

            Company = p.Company.Name,
            Department = p.Department != null ? p.Department.Name : null,

        });

        return Task.FromResult(new PagedResult<GetActiveJobPostingsQueryResponse>(response, request.page, request.pageSize, totalCount));
    }
}

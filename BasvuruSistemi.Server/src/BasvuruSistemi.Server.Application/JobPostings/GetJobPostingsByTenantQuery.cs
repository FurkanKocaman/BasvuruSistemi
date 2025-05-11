using BasvuruSistemi.Server.Application.FormTemplates;
using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.JobPostings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BasvuruSistemi.Server.Application.JobPostings;
public sealed record GetJobPostingsByTenantQuery(
    int page,
    int pageSize
    ) : IRequest<PagedResult<GetJobPostingsByTenantQueryResponse>>;

public sealed class GetJobPostingsByTenantQueryResponse
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

    public Guid CompanyId { get; set; }
    public string Company { get; set; } = default!;

    public Guid? DepartmentId { get; set; }
    public string? Department { get; set; }
}

internal sealed class GetJobPostingsByTenantQueryHandler(
    ICurrentUserService currentUserService,
    IJobPostingRepository jobPostingRepository
    ) : IRequestHandler<GetJobPostingsByTenantQuery, PagedResult<GetJobPostingsByTenantQueryResponse>>
{
    public Task<PagedResult<GetJobPostingsByTenantQueryResponse>> Handle(GetJobPostingsByTenantQuery request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;

        if(!tenantId.HasValue)
            return Task.FromResult(new PagedResult<GetJobPostingsByTenantQueryResponse>(new List<GetJobPostingsByTenantQueryResponse>(), 0, 0, 0));

        var jobPostings = jobPostingRepository.Where(p => !p.IsDeleted).Include(p => p.Company).Include(p => p.Department);

        var totalCount = jobPostings.Count();

        var pagedJobPostings = jobPostings
            .Skip((request.page - 1) * request.pageSize)
            .Take(request.pageSize)
            .ToList();

        var response = pagedJobPostings.Select(jobPosting => new GetJobPostingsByTenantQueryResponse
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
            EmploymentType = jobPosting.EmploymentType.ToString(),
            ExperienceLevelRequired = jobPosting.ExperienceLevelRequired.ToString(),
            SalaryRange = jobPosting.MinSalary.ToString() + jobPosting.Currency + jobPosting.MaxSalary.ToString() + jobPosting.Currency,
            SkillsRequired = jobPosting.SkillsRequired,

            ContactInfo = jobPosting.ContactInfo,
            IsPublic = jobPosting.IsPublic,

            CompanyId = jobPosting.CompanyId,
            Company = jobPosting.Company.Name,

            DepartmentId = jobPosting.DepartmentId,
            Department = jobPosting.Department?.Name,
        }).ToList();

        return Task.FromResult(new PagedResult<GetJobPostingsByTenantQueryResponse>(response, request.page, request.pageSize, totalCount));
    }
}

using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.JobPostings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.JobPostings;

public sealed record GetJobPostingsByTenantQuery(
    int page,
    int pageSize
) : IRequest<Result<PagedResult<GetJobPostingsByTenantQueryResponse>>>;

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

    public Guid? UnitId { get; set; }
    public string? Unit { get; set; } = default!;

    public int TotalApplicationsCount { get; set; }
}

internal sealed class GetJobPostingsByTenantQueryHandler(
    ICurrentUserService currentUserService,
    IJobPostingRepository jobPostingRepository
) : IRequestHandler<GetJobPostingsByTenantQuery, Result<PagedResult<GetJobPostingsByTenantQueryResponse>>>
{
    public async Task<Result<PagedResult<GetJobPostingsByTenantQueryResponse>>> Handle(GetJobPostingsByTenantQuery request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;

        if (!tenantId.HasValue)
        {
            return Result<PagedResult<GetJobPostingsByTenantQueryResponse>>.Failure(401, "Unauthorized");
        }

        var query = jobPostingRepository
            .Where(p => !p.IsDeleted && p.TenantId == tenantId)
            .Include(p => p.Unit)
            .Include(p => p.Applications);

        var totalCount = await query.CountAsync(cancellationToken);

        var pagedJobPostings = await query
            .Skip((request.page - 1) * request.pageSize)
            .Take(request.pageSize)
            .ToListAsync(cancellationToken);

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
            EmploymentType = jobPosting.EmploymentType?.ToString(),
            ExperienceLevelRequired = jobPosting.ExperienceLevelRequired?.ToString(),
            SalaryRange = $"{jobPosting.MinSalary}{jobPosting.Currency} - {jobPosting.MaxSalary}{jobPosting.Currency}",
            SkillsRequired = jobPosting.SkillsRequired,
            ContactInfo = jobPosting.ContactInfo,
            IsPublic = jobPosting.IsPublic,
            UnitId = jobPosting.UnitId,
            Unit = jobPosting.Unit?.Name,
            TotalApplicationsCount = jobPosting.Applications.Count
        }).ToList();

        var result = new PagedResult<GetJobPostingsByTenantQueryResponse>(
            response,
            request.page,
            request.pageSize,
            totalCount
        );

        return Result<PagedResult<GetJobPostingsByTenantQueryResponse>>.Succeed(result);
    }
}

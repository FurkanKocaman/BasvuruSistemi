using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.JobPostings;
using BasvuruSistemi.Server.Domain.PostingGroups;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.JobPostings;

public sealed record GetActiveJobPostingsSummariesQuery(
    int page,
    int pageSize
) : IRequest<Result<PagedResult<GetActiveJobPostingsSummariesQueryResponse>>>;

public sealed class GetActiveJobPostingsSummariesQueryResponse
{
    public Guid Id { get; set; }

    public string Title { get; set; } = default!;
    public int Type { get; set; }
    public string? Description { get; set; }
    public string? Qualifications { get; set; }

    public DateTimeOffset? ValidTo { get; set; }

    public string? LocationText { get; set; }

    public int? VacancyCount { get; set; }
    public string? SalaryRange { get; set; }

    public bool IsPublic { get; set; }

    public string Tenant { get; set; } = default!;
    public string? Unit { get; set; }
}

internal sealed class GetActiveJobPostingsSummariesQueryHandler(
    IJobPostingRepository jobPostingRepository,
    IPostingGroupRepository postingGroupRepository
) : IRequestHandler<GetActiveJobPostingsSummariesQuery, Result<PagedResult<GetActiveJobPostingsSummariesQueryResponse>>>
{
    public async Task<Result<PagedResult<GetActiveJobPostingsSummariesQueryResponse>>> Handle(GetActiveJobPostingsSummariesQuery request, CancellationToken cancellationToken)
    {
        var jobPostingsQuery = jobPostingRepository
            .Where(p => p.Status == JobPostingStatus.Published && p.IsPublic && !p.IsDeleted && p.PostingGroupId == null)
            .Include(p => p.Tenant)
            .Include(p => p.Unit);

        var postingGroupsQuery = postingGroupRepository
            .Where(p => p.Status == JobPostingStatus.Published && p.IsPublic && !p.IsDeleted)
            .Include(p => p.Tenant)
            .Include(p => p.Unit)
            .Include(p => p.JobPostings);

        var jobPostings = await jobPostingsQuery.ToListAsync(cancellationToken);
        var postingGroups = await postingGroupsQuery.ToListAsync(cancellationToken);

        var combined = new List<GetActiveJobPostingsSummariesQueryResponse>();

        combined.AddRange(jobPostings.Select(p => new GetActiveJobPostingsSummariesQueryResponse
        {
            Id = p.Id,
            Type = 0,
            Title = p.Title,
            Description = p.Description,
            Qualifications = p.Qualifications,
            ValidTo = p.ValidTo,
            LocationText = p.LocationText,
            VacancyCount = p.VacancyCount,
            SalaryRange = $"{p.MinSalary}{p.Currency} - {p.MaxSalary}{p.Currency}",
            IsPublic = p.IsPublic,
            Tenant = p.Tenant.Name,
            Unit = p.Unit?.Name
        }));

        combined.AddRange(postingGroups.Select(p => new GetActiveJobPostingsSummariesQueryResponse
        {
            Id = p.Id,
            Type = 1,
            Title = p.Name,
            Description = p.Description,
            Qualifications = null,
            ValidTo = p.OverallApplicationEndDate,
            LocationText = null,
            VacancyCount = p.JobPostings.Sum(x => x.VacancyCount ?? 0),
            SalaryRange = null,
            IsPublic = p.IsPublic,
            Tenant = p.Tenant.Name,
            Unit = p.Unit?.Name
        }));

        var totalCount = combined.Count;

        var paged = combined
            .OrderByDescending(p => p.ValidTo)
            .Skip((request.page - 1) * request.pageSize)
            .Take(request.pageSize)
            .ToList();

        var result = new PagedResult<GetActiveJobPostingsSummariesQueryResponse>(paged, request.page, request.pageSize, totalCount);

        return Result<PagedResult<GetActiveJobPostingsSummariesQueryResponse>>.Succeed(result);
    }
}

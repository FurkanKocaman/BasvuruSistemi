using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.JobPostings;
using BasvuruSistemi.Server.Domain.PostingGroups;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.JobPostings;

public sealed record GetJobPostingsSummariesByTenantQuery(
    int page,
    int pageSize
) : IRequest<Result<PagedResult<GetJobPostingsSummariesByTenantQueryResponse>>>;

public sealed class GetJobPostingsSummariesByTenantQueryResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public JobPostingType Type { get; set; } = default!;
    public DateTimeOffset? ValidFrom { get; set; }
    public DateTimeOffset? ValidTo { get; set; }
    public int? VacancyCount { get; set; }
    public int TotalApplicationsCount { get; set; }
    public bool IsPublic { get; set; }
    public JobPostingStatus Status { get; set; }
    public string? Unit { get; set; } = default!;
    public DateTimeOffset CreatedAt { get; set; }
}

internal sealed class GetJobPostingsSummariesByTenantQueryHandler(
    ICurrentUserService currentUserService,
    IJobPostingRepository jobPostingRepository,
    IPostingGroupRepository postingGroupRepository
) : IRequestHandler<GetJobPostingsSummariesByTenantQuery, Result<PagedResult<GetJobPostingsSummariesByTenantQueryResponse>>>
{
    public async Task<Result<PagedResult<GetJobPostingsSummariesByTenantQueryResponse>>> Handle(GetJobPostingsSummariesByTenantQuery request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;

        if (!tenantId.HasValue)
        {
            return Result<PagedResult<GetJobPostingsSummariesByTenantQueryResponse>>.Failure(401, "Unauthorized");
        }

        var jobPostings = await jobPostingRepository
            .Where(p => !p.IsDeleted && p.TenantId == tenantId.Value && p.PostingGroupId == null)
            .Include(p => p.Unit)
            .Include(p => p.Applications)
            .ToListAsync(cancellationToken);

        var postingGroups = await postingGroupRepository
            .Where(p => !p.IsDeleted && p.TenantId == tenantId.Value)
            .Include(p => p.JobPostings)
                .ThenInclude(p => p.Applications)
            .Include(p => p.Unit)
            .ToListAsync(cancellationToken);

        var response = jobPostings.Select(jobPosting => new GetJobPostingsSummariesByTenantQueryResponse
        {
            Id = jobPosting.Id,
            Title = jobPosting.Title,
            Type = JobPostingType.JobPosting,
            ValidFrom = jobPosting.ValidFrom,
            ValidTo = jobPosting.ValidTo,
            VacancyCount = jobPosting.VacancyCount,
            TotalApplicationsCount = jobPosting.Applications.Count,
            IsPublic = jobPosting.IsPublic,
            Status = jobPosting.Status,
            Unit = jobPosting.Unit?.Name,
            CreatedAt = jobPosting.CreatedAt
        }).ToList();

        var postingGroupsResponse = postingGroups.Select(postingGroup => new GetJobPostingsSummariesByTenantQueryResponse
        {
            Id = postingGroup.Id,
            Title = postingGroup.Name,
            Type = JobPostingType.PostingGroup,
            ValidFrom = postingGroup.OverallApplicationStartDate,
            ValidTo = postingGroup.OverallApplicationEndDate,
            VacancyCount = postingGroup.JobPostings.Sum(p => p.VacancyCount ?? 0),
            TotalApplicationsCount = postingGroup.JobPostings.Sum(p => p.Applications.Count),
            IsPublic = postingGroup.IsPublic,
            Status = postingGroup.Status,
            Unit = postingGroup.Unit?.Name,
            CreatedAt = postingGroup.CreatedAt
        }).ToList();

        response.AddRange(postingGroupsResponse);

        var totalCount = response.Count;
        var pagedJobPostings = response
            .Skip((request.page - 1) * request.pageSize)
            .Take(request.pageSize)
            .ToList();

        var pagedResult = new PagedResult<GetJobPostingsSummariesByTenantQueryResponse>(
            pagedJobPostings,
            request.page,
            request.pageSize,
            totalCount
        );

        return Result<PagedResult<GetJobPostingsSummariesByTenantQueryResponse>>.Succeed(pagedResult);
    }
}

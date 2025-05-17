using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.JobPostings;
using BasvuruSistemi.Server.Domain.PostingGroups;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BasvuruSistemi.Server.Application.JobPostings;
public sealed record GetJobPostingsSummariesByTenantQuery(
    int page,
    int pageSize
    ) : IRequest<PagedResult<GetJobPostingsSummariesByTenantQueryResponse>>;
public sealed class GetJobPostingsSummariesByTenantQueryResponse
{
    public Guid Id { get; set; }

    public string Title { get; set; } = default!;
    public string Type { get; set; } = default!;


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
    ) : IRequestHandler<GetJobPostingsSummariesByTenantQuery, PagedResult<GetJobPostingsSummariesByTenantQueryResponse>>
{
    public Task<PagedResult<GetJobPostingsSummariesByTenantQueryResponse>> Handle(GetJobPostingsSummariesByTenantQuery request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;

        if (!tenantId.HasValue)
            return Task.FromResult(new PagedResult<GetJobPostingsSummariesByTenantQueryResponse>(new List<GetJobPostingsSummariesByTenantQueryResponse>(), 0, 0, 0));

        var jobPostings = jobPostingRepository.Where(p => !p.IsDeleted && p.TenantId == tenantId.Value && p.PostingGroupId == null).Include(p => p.Unit).Include(p => p.Applications);

        var postingGroups = postingGroupRepository.Where(p => !p.IsDeleted && p.TenantId == tenantId.Value).Include(p => p.JobPostings).ThenInclude(p => p.Applications);

        var totalCount = jobPostings.Count() + postingGroups.Count();


        var response = jobPostings.Select(jobPosting => new GetJobPostingsSummariesByTenantQueryResponse
        {
            Id = jobPosting.Id,

            Title = jobPosting.Title,
            Type = "İlan",

            ValidFrom = jobPosting.ValidFrom,
            ValidTo = jobPosting.ValidTo,

            VacancyCount = jobPosting.VacancyCount,
            TotalApplicationsCount = jobPosting.Applications.Count(),

            IsPublic = jobPosting.IsPublic,
            Status = jobPosting.Status,

            Unit = jobPosting.Unit != null ? jobPosting.Unit.Name : null,

            CreatedAt = jobPosting.CreatedAt,

        }).ToList();

        var postingGroupsResponse = postingGroups.Select(postingGroup => new GetJobPostingsSummariesByTenantQueryResponse
        {
            Id = postingGroup.Id,

            Title = postingGroup.Name,
            Type = "İlan Grubu",

            ValidFrom = postingGroup.OverallApplicationStartDate,
            ValidTo = postingGroup.OverallApplicationEndDate,

            VacancyCount = postingGroup.JobPostings.Sum(p => p.VacancyCount),
            TotalApplicationsCount = postingGroup.JobPostings.Sum(p => p.Applications.Count()),

            IsPublic = postingGroup.IsPublic,
            Status = postingGroup.Status,

            Unit = postingGroup.Unit != null ? postingGroup.Unit.Name : null,

            CreatedAt = postingGroup.CreatedAt,

        }).ToList();

        response.AddRange(postingGroupsResponse);


        var pagedJobPostings = response
            .Skip((request.page - 1) * request.pageSize)
            .Take(request.pageSize)
            .ToList();

        return Task.FromResult(new PagedResult<GetJobPostingsSummariesByTenantQueryResponse>(pagedJobPostings, request.page, request.pageSize, totalCount));
    }
}

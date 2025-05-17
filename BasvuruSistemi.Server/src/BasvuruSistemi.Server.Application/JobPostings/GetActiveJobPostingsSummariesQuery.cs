using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Applications;
using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.JobPostings;
using BasvuruSistemi.Server.Domain.PostingGroups;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BasvuruSistemi.Server.Application.JobPostings;
public sealed record GetActiveJobPostingsSummariesQuery(
    int page,
    int pageSize
    ) : IRequest<PagedResult<GetActiveJobPostingsSummariesQueryResponse>>;
public sealed class GetActiveJobPostingsSummariesQueryResponse
{
    public Guid Id { get; set; }

    public string Title { get; set; } = default!;
    public string? Description { get; set; } = default!;
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
    ) : IRequestHandler<GetActiveJobPostingsSummariesQuery, PagedResult<GetActiveJobPostingsSummariesQueryResponse>>
{
    public Task<PagedResult<GetActiveJobPostingsSummariesQueryResponse>> Handle(GetActiveJobPostingsSummariesQuery request, CancellationToken cancellationToken)
    {
        var jobPostings = jobPostingRepository
            .Where(p => p.Status == JobPostingStatus.Published && p.IsPublic && !p.IsDeleted && p.PostingGroupId == null).Include(p => p.Unit);

        var postingGroups = postingGroupRepository.Where(p => p.Status == JobPostingStatus.Published && p.IsPublic && !p.IsDeleted).Include(p => p.JobPostings);

        var totalCount = jobPostings.Count() + postingGroups.Count();



        var response = jobPostings.Select(p => new GetActiveJobPostingsSummariesQueryResponse
        {
            Id = p.Id,

            Title = p.Title,
            Description = p.Description,
            Qualifications = p.Qualifications,

            ValidTo = p.ValidTo,

            LocationText = p.LocationText,

            VacancyCount = p.VacancyCount,
            SalaryRange = p.MinSalary.ToString() + p.Currency + p.MaxSalary.ToString() + p.Currency,

            IsPublic = p.IsPublic,

            Tenant = p.Tenant.Name,
            Unit = p.Unit != null ? p.Unit.Name : null,
        }).ToList();

        var postingGroupResponse = postingGroups.Select(p => new GetActiveJobPostingsSummariesQueryResponse
        {
            Id=p.Id,
            Title = p.Name,
            Description = p.Description,
            Qualifications = null,

            ValidTo = p.OverallApplicationEndDate,

            VacancyCount = p.JobPostings.Sum(p => p.VacancyCount),
            SalaryRange = null,

            IsPublic = p.IsPublic,

            Tenant = p.Tenant.Name,
            Unit = p.Unit != null ? p.Unit.Name : null,
        });

        response.AddRange(postingGroupResponse);

        var pagedJobPostings = response.OrderByDescending(p => p.ValidTo)
        .Skip((request.page - 1) * request.pageSize)
        .Take(request.pageSize)
        .ToList();

        return Task.FromResult(new PagedResult<GetActiveJobPostingsSummariesQueryResponse>(pagedJobPostings, request.page, request.pageSize, totalCount));
    }
}


using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.PostingGroups;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.PostingGroups;

public sealed record PostingGroupGetQuery(
    Guid id
) : IRequest<Result<PostingGroupGetQueryResponse>>;

public sealed class PostingGroupGetQueryResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;
    public string? Description { get; set; }

    public bool IsPublic { get; set; }
    public JobPostingStatus Status { get; set; }

    public DateTimeOffset? AnnouncementDate { get; set; }
    public DateTimeOffset? OverallApplicationStartDate { get; set; }
    public DateTimeOffset? OverallApplicationEndDate { get; set; }

    public Guid? ParentPostingGroupId { get; set; }
    public string? ParentPostingGroup { get; set; }

    public Guid TenantId { get; set; }
    public string Tenant { get; set; } = default!;

    public Guid? UnitId { get; set; }
    public string? Unit { get; set; }

    public List<JobPostingSummaryDto> JobPostings { get; set; } = new();
}

internal sealed class PostingGroupGetQueryHandler(
    IPostingGroupRepository postingGroupRepository
) : IRequestHandler<PostingGroupGetQuery, Result<PostingGroupGetQueryResponse>>
{
    public async Task<Result<PostingGroupGetQueryResponse>> Handle(PostingGroupGetQuery request, CancellationToken cancellationToken)
    {
        var postingGroup = await postingGroupRepository
            .Where(p => p.Id == request.id && !p.IsDeleted)
            .Include(p => p.JobPostings).ThenInclude(p => p.Applications)
            .Include(p => p.JobPostings).ThenInclude(p => p.Unit)
            .Include(p => p.Tenant)
            .Include(p => p.Unit)
            .Include(p => p.ParentPostingGroup)
            .FirstOrDefaultAsync(cancellationToken);

        if (postingGroup == null)
            return Result<PostingGroupGetQueryResponse>.Failure("İlan grubu bulunamadı.");

        var response = new PostingGroupGetQueryResponse
        {
            Id = postingGroup.Id,
            Name = postingGroup.Name,
            Description = postingGroup.Description,
            IsPublic = postingGroup.IsPublic,
            Status = postingGroup.Status,

            AnnouncementDate = postingGroup.AnnouncementDate,
            OverallApplicationStartDate = postingGroup.OverallApplicationStartDate,
            OverallApplicationEndDate = postingGroup.OverallApplicationEndDate,

            ParentPostingGroupId = postingGroup.ParentPostingGroupId,
            ParentPostingGroup = postingGroup.ParentPostingGroup?.Name,

            TenantId = postingGroup.TenantId,
            Tenant = postingGroup.Tenant.Name,

            UnitId = postingGroup.UnitId,
            Unit = postingGroup.Unit?.Name,

            JobPostings = postingGroup.JobPostings.Select(jobPosting => new JobPostingSummaryDto(
                jobPosting.Id,
                jobPosting.Title,
                jobPosting.Description,
                jobPosting.Responsibilities,
                jobPosting.Qualifications,
                jobPosting.ValidFrom,
                jobPosting.ValidTo,
                jobPosting.Applications.Count(),
                jobPosting.Status,
                jobPosting.Unit?.Name
            )).ToList()
        };

        return Result<PostingGroupGetQueryResponse>.Succeed(response);
    }
}

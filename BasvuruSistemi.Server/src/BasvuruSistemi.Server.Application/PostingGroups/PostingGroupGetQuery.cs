using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.PostingGroups;
using BasvuruSistemi.Server.Domain.Tenants;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BasvuruSistemi.Server.Application.PostingGroups;
public sealed record PostingGroupGetQuery(
    Guid id
    ) : IRequest<PostingGroupGetQueryResponse>;

public sealed class PostingGroupGetQueryResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;
    public string? Description { get; set; } 

    public bool IsPublic { get; set; }
    public JobPostingStatus Status { get; set; }

    public DateTimeOffset? AnnouncementDate { get;  set; }
    public DateTimeOffset? OverallApplicationStartDate { get;  set; }
    public DateTimeOffset? OverallApplicationEndDate { get;  set; }

    public Guid? ParentPostingGroupId { get; set; }
    public string? ParentPostingGroup { get; set; }

    public Guid TenantId { get; set; }
    public string Tenant { get; set; } = default!;

    public Guid? UnitId { get; set; }
    public string? Unit { get; set; }

    public List<JobPostingSummaryDto> JobPostings { get; set; } = new List<JobPostingSummaryDto>();
}

internal sealed class PostingGroupGetQueryHandler(
    IPostingGroupRepository postingGroupRepository
    ) : IRequestHandler<PostingGroupGetQuery, PostingGroupGetQueryResponse>
{
    public Task<PostingGroupGetQueryResponse> Handle(PostingGroupGetQuery request, CancellationToken cancellationToken)
    {
        var postingGroup = postingGroupRepository.Where(p => p.Id == request.id && !p.IsDeleted)
            .Include(p => p.JobPostings)
                .ThenInclude(p => p.Applications)
            .Include(p => p.JobPostings)
                .ThenInclude(p => p.Unit)
            .Include(p => p.Tenant)
            .Include(p => p.Unit)
            .Include(p => p.ParentPostingGroup)
            .FirstOrDefault();

        if (postingGroup == null)
            return Task.FromResult(new PostingGroupGetQueryResponse());

        var response = new PostingGroupGetQueryResponse()
        {
            Id = postingGroup.Id,

            Name = postingGroup.Name,
            Description = postingGroup.Description,

            IsPublic = postingGroup.IsPublic,
            Status = postingGroup.Status ,

            AnnouncementDate = postingGroup.AnnouncementDate,
            OverallApplicationStartDate = postingGroup.OverallApplicationStartDate,
            OverallApplicationEndDate = postingGroup.OverallApplicationEndDate,

            ParentPostingGroupId = postingGroup.ParentPostingGroupId,
            ParentPostingGroup = postingGroup.ParentPostingGroup != null ? postingGroup.ParentPostingGroup.Name : null,

            TenantId = postingGroup.TenantId,
            Tenant = postingGroup.Tenant.Name,

            UnitId = postingGroup.UnitId,
            Unit = postingGroup.Unit != null ? postingGroup.Unit.Name : null,

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
                jobPosting.Unit != null ? jobPosting.Unit.Name : null
            )).ToList()
        };


        return Task.FromResult(response);
    }
}

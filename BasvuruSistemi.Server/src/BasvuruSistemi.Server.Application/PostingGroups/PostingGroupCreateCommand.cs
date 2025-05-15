using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.PostingGroups;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.PostingGroups;
public sealed record PostingGroupCreateCommand(
    string name,
    string? description,
    DateTimeOffset? announcementDate,
    DateTimeOffset? overallApplicationStartDate,
    DateTimeOffset? overallApplicationEndDate
    ) : IRequest<Result<string>>;

internal sealed class PostingGroupCreateCommandHandler(
    ICurrentUserService currentUserService,
    IPostingGroupRepository postingGroupRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<PostingGroupCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(PostingGroupCreateCommand request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;
        if (!tenantId.HasValue)
            return Result<string>.Failure("Tenant not found");

        var postingGroup = new PostingGroup(
            tenantId.Value,
            request.name,
            request.description,
            announcementDate: request.announcementDate,
            overallApplicationStartDate: request.overallApplicationStartDate,
            overallApplicationEndDate: request.overallApplicationEndDate
        );

        postingGroupRepository.Add(postingGroup);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Posting group created successfully");
    }
}

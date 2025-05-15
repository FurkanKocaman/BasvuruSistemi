using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Applications;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Applications;
public sealed record CheckApplicationExistsQuery(
    Guid jobPostingId
    ) : IRequest<Result<bool>>;

internal sealed class CheckApplicationExistsQueryHandler(
    ICurrentUserService currentUserService,
    IApplicationRepository applicationRepository
    ) : IRequestHandler<CheckApplicationExistsQuery, Result<bool>>
{
    public Task<Result<bool>> Handle(CheckApplicationExistsQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        if (!userId.HasValue)
            return Task.FromResult(Result<bool>.Failure("User not found"));

        var applicationExist = applicationRepository.Any(p => p.UserId == userId.Value && p.JobPostingId == request.jobPostingId && !p.IsDeleted);

        if (applicationExist)
            return Task.FromResult(Result<bool>.Failure("Bu ilana zaten başvurunuz bulunuyor."));

        return Task.FromResult(Result<bool>.Succeed(true));
    }
}

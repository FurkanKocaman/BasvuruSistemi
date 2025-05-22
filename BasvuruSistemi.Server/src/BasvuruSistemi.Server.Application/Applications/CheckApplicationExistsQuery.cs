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
    public async Task<Result<bool>> Handle(CheckApplicationExistsQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        if (!userId.HasValue)
            return Result<bool>.Failure("User not found");

        var applicationExist = await applicationRepository.AnyAsync(p => p.UserId == userId.Value && p.JobPostingId == request.jobPostingId && !p.IsDeleted);

        if (applicationExist)
            return Result<bool>.Failure("Bu ilana zaten başvurunuz bulunuyor.");

        return Result<bool>.Succeed(true);
    }
}

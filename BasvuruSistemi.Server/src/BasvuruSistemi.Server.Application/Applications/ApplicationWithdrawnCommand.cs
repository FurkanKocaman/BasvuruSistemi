using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Applications;
using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Applications;
public sealed record ApplicationWithdrawnCommand(
    Guid applicationId
    ) : IRequest<Result<string>>;

internal sealed class ApplicationWithdrawnCommandHandler(
    IApplicationRepository applicationRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService
    ) : IRequestHandler<ApplicationWithdrawnCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ApplicationWithdrawnCommand request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;

        if (!userId.HasValue)
            return Result<string>.Failure("User not found");

        var application = await applicationRepository.FirstOrDefaultAsync(p => p.Id == request.applicationId && p.UserId == userId.Value);
        if (application is null)
            return Result<string>.Failure("Application not found");

        if (application.Status != ApplicationStatus.Pending)
            return Result<string>.Failure("This application already reviewed");

        application.Withdrawn();

        applicationRepository.Update(application);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Application withdrawn successfully");
    }
}

using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Applications;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Applications;
public sealed record ApplicationDeleteCommand(
    Guid applicationId
    ) : IRequest<Result<string>>;

internal sealed class ApplicationDeleteCommandHandler(
    ICurrentUserService currentUserService,
    IApplicationRepository applicationRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<ApplicationDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ApplicationDeleteCommand request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        if (userId is null)
            return Result<string>.Failure(401, "Unauthorized");

        var application = applicationRepository.Where(p => p.Id == request.applicationId && !p.IsDeleted).FirstOrDefault();
        if (application is null)
            return Result<string>.Failure(404, "Application not found or already deleted");

        if (application.UserId != userId)
            return Result<string>.Failure(403, "You are not authorized to delete this application");

        application.IsDeleted = true;

        applicationRepository.Update(application);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Application deleted successfully");
    }
}

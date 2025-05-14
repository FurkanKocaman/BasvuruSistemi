using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Applications;
using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Applications;
public sealed record ApplicationReviewCommand(
    Guid id,
    int status,
    string? reviewDescription
    ) : IRequest<Result<string>>;

internal sealed class ApplicationReviewCommandHandler(
    ICurrentUserService currentUserService,
    IApplicationRepository applicationRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<ApplicationReviewCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ApplicationReviewCommand request, CancellationToken cancellationToken)
    {
        Guid? userıd = currentUserService.UserId;

        if (!userıd.HasValue)
            return Result<string>.Failure("user not found");

        var application = await applicationRepository.FirstOrDefaultAsync(p => p.Id == request.id);

        if (application is null)
            return Result<string>.Failure("Application not found");
    
        if(application.Status != ApplicationStatus.Pending)
            return Result<string>.Failure("This application already reviewed");

        if (!System.Enum.IsDefined(typeof(ApplicationStatus), request.status))
        {
            return Result<string>.Failure($"Invalid ApplicationStatus: {request.status}.");
        }
        ApplicationStatus status = (ApplicationStatus)request.status;

        application.Review(status, request.reviewDescription,userıd.Value);

        applicationRepository.Update(application);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Application reviewed successfully");
    }
}

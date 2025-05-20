using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Entities;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Entities;
public sealed record CertificationCreateCommand(
    string title,
    string issuer,
    DateOnly received,
    DateOnly? expiry = null
    ) : IRequest<Result<string>>;


internal sealed class CertificationCreateCommandHandler(
    ICertificationRepository certificationRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<CertificationCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CertificationCreateCommand request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        if (!userId.HasValue)
            return Result<string>.Failure(404, "User not found");

        var certification = new Certification(
            userId.Value,
            request.title,
            request.issuer,
            request.received,
            request.expiry);

        certificationRepository.Add(certification);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Certification created successfully");
    }
}

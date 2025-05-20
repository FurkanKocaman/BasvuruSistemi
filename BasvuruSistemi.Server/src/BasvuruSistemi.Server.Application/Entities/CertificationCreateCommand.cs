using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.Entities;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Entities;
public sealed record CertificationCreateCommand(
    string Title,
    string Issuer,
    DateOnly DateReceived,
    DateOnly? ExpiryDate = null
    ) : IRequest<Result<CertificationDto>>;


internal sealed class CertificationCreateCommandHandler(
    ICertificationRepository certificationRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<CertificationCreateCommand, Result<CertificationDto>>
{
    public async Task<Result<CertificationDto>> Handle(CertificationCreateCommand request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        if (!userId.HasValue)
            return Result<CertificationDto>.Failure(404, "User not found");

        var certification = new Certification(
            userId.Value,
            request.Title,
            request.Issuer,
            request.DateReceived,
            request.ExpiryDate);

        certificationRepository.Add(certification);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<CertificationDto>.Succeed(new CertificationDto(certification.Id,certification.Title,certification.Issuer,certification.DateReceived,certification.ExpiryDate));
    }
}

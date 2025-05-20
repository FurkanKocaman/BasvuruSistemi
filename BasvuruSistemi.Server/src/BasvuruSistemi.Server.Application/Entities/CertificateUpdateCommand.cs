using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.Entities;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Entities;
public sealed record CertificateUpdateCommand(
    CertificationDto Certificate
    ) : IRequest<Result<string>>;

internal sealed class CertificateUpdateCommandHandler(
    ICertificationRepository certificationRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<CertificateUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CertificateUpdateCommand request, CancellationToken cancellationToken)
    {
        var certificate = await certificationRepository.FirstOrDefaultAsync(p => p.Id == request.Certificate.Id);
        if(certificate is null)
            return Result<string>.Failure("Certificate not found");

        certificate.Update(
            request.Certificate.Title,
            request.Certificate.Issuer,
            request.Certificate.DateReceived,
            request.Certificate.ExpiryDate
            );

        certificationRepository.Update(certificate);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Certificate updated successfully");
    }
}


using BasvuruSistemi.Server.Domain.Entities;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Entities;
public sealed record CertificationDeleteCommand(
    Guid id
    ) : IRequest<Result<string>>;

internal sealed class CertificationDeleteCommandHandler(
    ICertificationRepository certificationRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<CertificationDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CertificationDeleteCommand request, CancellationToken cancellationToken)
    {
        var certification = await certificationRepository.FirstOrDefaultAsync(p => p.Id == request.id);
        if (certification == null)
            return Result<string>.Failure(404,"Certification not found");

        certificationRepository.Delete(certification);

        await unitOfWork.SaveChangesAsync();
        return Result<string>.Succeed("Certification deleted successfully");
    }
}

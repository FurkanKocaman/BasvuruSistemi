using BasvuruSistemi.Server.Domain.Entities;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Entities;
public sealed record EducationDeleteCommand(
    Guid id
    ) : IRequest<Result<string>>;

internal sealed class EducationDeleteCommandHandler(
    IEducationRepository educationRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<EducationDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(EducationDeleteCommand request, CancellationToken cancellationToken)
    {
        var education = await educationRepository.FirstOrDefaultAsync(p => p.Id == request.id);
        if (education is null)
            return Result<string>.Failure(404, "Education not found");

        educationRepository.Delete(education);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<string>.Succeed("Education deleted successfully");
    }
}

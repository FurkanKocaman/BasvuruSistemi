using BasvuruSistemi.Server.Domain.UnitOfWork;
using BasvuruSistemi.Server.Domain.Units;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Units;
public sealed record UnitDeleteCommand(
    Guid id
    ) : IRequest<Result<string>>;

internal sealed class UnitDeleteCommandHandler(
    IUnitRepository unitRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<UnitDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UnitDeleteCommand request, CancellationToken cancellationToken)
    {
        var unit = await unitRepository.FirstOrDefaultAsync(p => p.Id == request.id);

        if (unit is null)
            return Result<string>.Failure("unit not found");

        unit.IsDeleted = true;

        unitRepository.Update(unit);

        await unitOfWork.SaveChangesAsync();

        return Result<string>.Succeed("Unit deleted successfully");
    }
}

using BasvuruSistemi.Server.Domain.UnitOfWork;
using BasvuruSistemi.Server.Domain.Units;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Units;
public sealed record UnitUpdateCommand(
    Guid id,
    Guid? parentId,
    string name,
    string? code
    ) : IRequest<Result<string>>;

internal sealed class UnitUpdateCommandhandler(
    IUnitRepository unitRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<UnitUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UnitUpdateCommand request, CancellationToken cancellationToken)
    {
        var unit = await unitRepository.FirstOrDefaultAsync(p => p.Id == request.id);

        if (unit is null)
            return Result<string>.Failure("unit not found");

        unit.Update(request.name, request.code, request.parentId);

        unitRepository.Update(unit);


        await unitOfWork.SaveChangesAsync();

        return Result<string>.Succeed("Unit updated successfully");
    }
}

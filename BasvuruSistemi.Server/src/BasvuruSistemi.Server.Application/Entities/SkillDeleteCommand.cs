using BasvuruSistemi.Server.Domain.Entities;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Entities;
public sealed record SkillDeleteCommand(
    Guid id
    ) : IRequest<Result<string>>;

internal sealed class SkillDeleteCommandHandler(
    ISkillRepository skillRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<SkillDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(SkillDeleteCommand request, CancellationToken cancellationToken)
    {
        var skill = await skillRepository.FirstOrDefaultAsync(p => p.Id == request.id);
        if (skill == null)
            return Result<string>.Failure(404, "Skill not found");

        skillRepository.Delete(skill);

        await unitOfWork.SaveChangesAsync();
        return Result<string>.Succeed("Skill deleted successfully");
    }
}
   
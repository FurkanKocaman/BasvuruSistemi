using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.Entities;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Entities;
public sealed record SkillUpdateCommand(
    SkillDto Skill
    ) : IRequest<Result<string>>;

internal sealed class SkillUpdateCommandHandler(
    ISkillRepository skillRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<SkillUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(SkillUpdateCommand request, CancellationToken cancellationToken)
    {
        var skill = await skillRepository.FirstOrDefaultAsync(p => p.Id == request.Skill.Id);
        if (skill is null)
            return Result<string>.Failure(404, "Skill not found");

        skill.Update(
            request.Skill.Name,
            request.Skill.Description,
            request.Skill.level
            );

        skillRepository.Update(skill);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Skill updated successfully");
    }
}

using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.Entities;
using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Entities;
public sealed record SkillCreateCommand(
    string Name,
    string? Description,
    SkillLevel level
    ) : IRequest<Result<SkillDto>>;

internal sealed class SkillCreateCommandHandler(
    ISkillRepository skillRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<SkillCreateCommand, Result<SkillDto>>
{
    public async Task<Result<SkillDto>> Handle(SkillCreateCommand request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        if (!userId.HasValue)
            return Result<SkillDto>.Failure(404, "User not found");
        
        var skill = new Skill(
            userId.Value,
            request.Name,
            request.Description,
            request.level);

        skillRepository.Add(skill);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<SkillDto>.Succeed(new SkillDto(skill.Id,skill.Name,skill.Description,skill.Level));
    }
}

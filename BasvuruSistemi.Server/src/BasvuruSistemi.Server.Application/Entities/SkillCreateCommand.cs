using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Entities;
using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Entities;
public sealed record SkillCreateCommand(
    string name,
    string? description,
    SkillLevel level
    ) : IRequest<Result<string>>;

internal sealed class SkillCreateCommandHandler(
    ISkillRepository skillRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<SkillCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(SkillCreateCommand request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        if (!userId.HasValue)
            return Result<string>.Failure(404, "User not found");
        
        var skill = new Skill(
            userId.Value,
            request.name,
            request.description,
            request.level);

        skillRepository.Add(skill);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Skill created successfully");
    }
}

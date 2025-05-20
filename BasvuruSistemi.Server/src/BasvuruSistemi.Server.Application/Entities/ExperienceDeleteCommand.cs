using BasvuruSistemi.Server.Domain.Entities;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Entities;
public sealed record ExperienceDeleteCommand(
    Guid id
    ) : IRequest<Result<string>>;

internal sealed class ExperienceDeleteCommandHandler(
    IExperienceRepository experienceRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<ExperienceDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ExperienceDeleteCommand request, CancellationToken cancellationToken)
    {
        var experience = await experienceRepository.FirstOrDefaultAsync(p => p.Id == request.id);
        if (experience == null)
            return Result<string>.Failure(404, "Experience not found");

        experienceRepository.Delete(experience);

        await unitOfWork.SaveChangesAsync();
        return Result<string>.Succeed("Experience deleted successfully");
    }
}
using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.Entities;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Entities;
public sealed record ExperienceUpdateCommand(
    ExperienceDto Experience
    ): IRequest<Result<string>>;

internal sealed class ExperienceUpdateCommandHandler(
    IExperienceRepository experienceRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<ExperienceUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ExperienceUpdateCommand request, CancellationToken cancellationToken)
    {
        var experience = await experienceRepository.FirstOrDefaultAsync(p => p.Id == request.Experience.Id);
        if (experience is null)
            return Result<string>.Failure(404,"Experience not found");

        experience.Update(
            request.Experience.CompanyName,
            request.Experience.Position,
            request.Experience.StartDate,
            request.Experience.EndDate,
            request.Experience.Description,
            request.Experience.Location
            );

        experienceRepository.Update(experience);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Experience updated successfully");
    }
}

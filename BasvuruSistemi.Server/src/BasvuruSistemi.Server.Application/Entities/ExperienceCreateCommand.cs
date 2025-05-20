using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Entities;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Entities;
public sealed record ExperienceCreateCommand(
     string company,
     string position,
     DateOnly start, 
     DateOnly? end = null,
     string? description = null,
     string? location = null
    ) : IRequest<Result<string>>;

internal sealed class ExperienceCreateCommandHandler(
    IExperienceRepository experienceRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<ExperienceCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ExperienceCreateCommand request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;

        if (!userId.HasValue)
            return Result<string>.Failure("User not found");

        var experience = new Experience(
            userId.Value,
            request.company,
            request.position,
            request.start,
            request.end,
            request.description,
            request.location);

        experienceRepository.Add(experience);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Experience created successfully");

    }
}

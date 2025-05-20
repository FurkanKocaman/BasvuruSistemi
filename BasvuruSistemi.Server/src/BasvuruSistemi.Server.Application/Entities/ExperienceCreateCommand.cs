using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.Entities;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Entities;
public sealed record ExperienceCreateCommand(
     string CompanyName,
     string Position,
     DateOnly StartDate, 
     DateOnly? EndDate = null,
     string? Description = null,
     string? Location = null
    ) : IRequest<Result<ExperienceDto>>;

internal sealed class ExperienceCreateCommandHandler(
    IExperienceRepository experienceRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<ExperienceCreateCommand, Result<ExperienceDto>>
{
    public async Task<Result<ExperienceDto>> Handle(ExperienceCreateCommand request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;

        if (!userId.HasValue)
            return Result<ExperienceDto>.Failure(404,"User not found");

        var experience = new Experience(
            userId.Value,
            request.CompanyName,
            request.Position,
            request.StartDate,
            request.EndDate,
            request.Description,
            request.Location);

        experienceRepository.Add(experience);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<ExperienceDto>.Succeed(new ExperienceDto(experience.Id,experience.CompanyName,experience.Position,experience.Location,experience.StartDate,experience.EndDate,experience.Description));

    }
}

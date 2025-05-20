using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.Entities;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Entities;
public sealed record EducationCreateCommand(
    string Institution, 
    string Department,  
    DateOnly StartDate,
    DateOnly? GraduationDate, 
    double? GPA = null, 
    string? Degree = null,
    string? Description = null
    ) : IRequest<Result<EducationDto>>;

internal sealed class EducationCreateCommandHandler(
    ICurrentUserService currentUserService,
    IEducationRepository educationRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<EducationCreateCommand, Result<EducationDto>>
{
    public async Task<Result<EducationDto>> Handle(EducationCreateCommand request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        if (!userId.HasValue)
            return Result<EducationDto>.Failure(404, "User not found");

        var education = new Education(
            userId.Value,
            request.Institution,
            request.Department,
            request.StartDate,
            request.GraduationDate,
            request.GPA,
            request.Degree,
            request.Description);

        educationRepository.Add(education);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<EducationDto>.Succeed(new EducationDto(education.Id,education.Institution,education.Department,education.Degree,education.Description,education.StartDate,education.GraduationDate,education.GPA));
    }
}

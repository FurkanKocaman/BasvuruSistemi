using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.Entities;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Entities;
public sealed record EducationUpdateCommand(
    EducationDto Education
    ) : IRequest<Result<string>>;

internal sealed class EducationUpdateCommandHandler(
    IEducationRepository educationRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<EducationUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(EducationUpdateCommand request, CancellationToken cancellationToken)
    {
        var education = await educationRepository.FirstOrDefaultAsync(p => p.Id == request.Education.Id);

        if (education is null)
            return Result<string>.Failure(404, "Education not found");

        education.Update(
            request.Education.Institution,
            request.Education.Department,
            request.Education.StartDate,
            request.Education.GraduationDate,
            request.Education.GPA,
            request.Education.Degree,
            request.Education.Description
            );

        educationRepository.Update(education);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Education updated successfully");
    }
}

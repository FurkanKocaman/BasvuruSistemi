using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Entities;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Entities;
public sealed record EducationCreateCommand(
    string institution, 
    string department,
    DateOnly start, 
    DateOnly grad, 
    double? gpa = null, 
    string? degree = null,
    string? description = null
    ) : IRequest<Result<string>>;

internal sealed class EducationCreateCommandHandler(
    ICurrentUserService currentUserService,
    IEducationRepository educationRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<EducationCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(EducationCreateCommand request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        if (!userId.HasValue)
            return Result<string>.Failure(404, "User not found");

        var education = new Education(
            userId.Value,
            request.institution,
            request.department,
            request.start,
            request.grad,
            request.gpa,
            request.degree,
            request.description);

        educationRepository.Add(education);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Education created successfully");
    }
}

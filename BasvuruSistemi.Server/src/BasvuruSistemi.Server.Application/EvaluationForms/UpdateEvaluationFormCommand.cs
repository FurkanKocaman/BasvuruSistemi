using BasvuruSistemi.Server.Domain.Evaluations;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.EvaluationForms;
public sealed record UpdateEvaluationFormCommand(
    Guid Id,
    string Name,
    string? Description
    ) : IRequest<Result<string>>;

internal sealed class UpdateEvaluationFormCommandHandler(
    IEvaluationFormRepository evaluationFormRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<UpdateEvaluationFormCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateEvaluationFormCommand request, CancellationToken cancellationToken)
    {
        var evaluationForm = await evaluationFormRepository.FirstOrDefaultAsync(p => p.Id ==request.Id,cancellationToken);

        if(evaluationForm is null)
            return Result<string>.Failure(404, "Evaluation form not found.");

        evaluationForm.Update(request.Name, request.Description);
        evaluationFormRepository.Update(evaluationForm);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed(evaluationForm.Id.ToString());
    }
}

using BasvuruSistemi.Server.Domain.Evaluations;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.EvaluationFormFields;
public sealed record DeleteFormFieldFromEvaluationFormCommand(
    Guid Id
    ) : IRequest<Result<string>>;

internal sealed class DeleteFormFieldFromEvaluationFormCommandHandler(
    IEvaluationFormFieldDefinitionRepository evaluationFormFieldDefinitionRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<DeleteFormFieldFromEvaluationFormCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteFormFieldFromEvaluationFormCommand request, CancellationToken cancellationToken)
    {
        var evaluationFormFieldDefinition = await evaluationFormFieldDefinitionRepository.FirstOrDefaultAsync(p => p.Id == request.Id);
        if(evaluationFormFieldDefinition is null)
        {
            return Result<string>.Failure(404,$"Evaluation form field with ID {request.Id} not found.");
        }

        evaluationFormFieldDefinitionRepository.Delete(evaluationFormFieldDefinition);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Evaluation form field deleted successfully.");
    }
}

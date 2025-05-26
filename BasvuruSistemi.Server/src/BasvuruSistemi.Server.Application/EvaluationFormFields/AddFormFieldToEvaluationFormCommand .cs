using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.Evaluations;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.EvaluationForms;
public sealed record AddFormFieldToEvaluationFormCommand(
    Guid EvaluationFormId,
    string Label,
    int FieldType,
    string? Options = null,
    bool IsRequired = false,
    int Order = 0,
    string? Placeholder = null,
    string? HelpText = null,
    string? ValidationRules = null
    ) : IRequest<Result<string>>;

internal sealed class AddFormFieldToEvaluationFormCommandHandler(
    IEvaluationFormFieldDefinitionRepository evaluationFormFieldDefinitionRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<AddFormFieldToEvaluationFormCommand, Result<string>>
{
    public async Task<Result<string>> Handle(AddFormFieldToEvaluationFormCommand request, CancellationToken cancellationToken)
    {
        if (!Enum.IsDefined(typeof(FieldTypeEnum), request.FieldType))
        {
            return Result<string>.Failure($"Invalid FielType: {request.FieldType}.");
        }
        var fieldType = (FieldTypeEnum)request.FieldType;

        var evaluationFormFieldDefinition = new EvaluationFormFieldDefinition(
            request.EvaluationFormId,
            request.Label,
            fieldType,
            request.Options,
            request.IsRequired,
            request.Order,
            request.Placeholder,
            request.HelpText,
            request.ValidationRules
        );
        evaluationFormFieldDefinitionRepository.Add(evaluationFormFieldDefinition);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed(evaluationFormFieldDefinition.Id.ToString());
    }
}

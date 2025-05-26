using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.Evaluations;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.EvaluationFormFields;
public sealed record UpdateFormFieldByEvaluationFormCommand(
    Guid Id,
    string Label,
    int FieldType,
    string? Options = null,
    bool IsRequired = false,
    int Order = 0,
    string? Placeholder = null,
    string? HelpText = null,
    string? ValidationRules = null
    ) : IRequest<Result<string>>;

internal sealed class UpdateFormFieldByEvaluationFormCommandHandler(
    IEvaluationFormFieldDefinitionRepository evaluationFormFieldDefinitionRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<UpdateFormFieldByEvaluationFormCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateFormFieldByEvaluationFormCommand request, CancellationToken cancellationToken)
    {
        var evaluationFormFieldDefinition = await evaluationFormFieldDefinitionRepository.FirstOrDefaultAsync(p => p.Id == request.Id);
        if (evaluationFormFieldDefinition == null)
        {
            return Result<string>.Failure(404,"Form field not found.");
        }
        if (!Enum.IsDefined(typeof(FieldTypeEnum), request.FieldType))
        {
            return Result<string>.Failure(400,$"Invalid FielType: {request.FieldType}.");
        }
        var fieldType = (FieldTypeEnum)request.FieldType;

        evaluationFormFieldDefinition.Update(
            request.Label,
            fieldType,
            request.Options,
            request.IsRequired,
            request.Order,
            request.Placeholder,
            request.HelpText,
            request.ValidationRules
        );

        evaluationFormFieldDefinitionRepository.Update(evaluationFormFieldDefinition);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Form field updated successfully.");
    }
}

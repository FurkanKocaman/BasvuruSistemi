using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.Evaluations;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.EvaluationForms;
public sealed record AddFormFieldToEvaluationFormCommand(
    Guid EvaluationFormId,
    string Label,
    string? Description,
    int Type,
    bool IsRequired,
    int Order,
    string? Placeholder,
    string? OptionsJson,
    bool IsReadonly,
    string? DefaultValue,
    string? AllowedFileTypes,
    int? MaxFileSizeMb
    ) : IRequest<Result<string>>;

internal sealed class AddFormFieldToEvaluationFormCommandHandler(
    IEvaluationFormFieldDefinitionRepository evaluationFormFieldDefinitionRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<AddFormFieldToEvaluationFormCommand, Result<string>>
{
    public async Task<Result<string>> Handle(AddFormFieldToEvaluationFormCommand request, CancellationToken cancellationToken)
    {
        if (!Enum.IsDefined(typeof(FieldTypeEnum), request.Type))
        {
            return Result<string>.Failure($"Invalid FielType: {request.Type}.");
        }
        var fieldType = (FieldTypeEnum)request.Type;

        var evaluationFormFieldDefinition = new EvaluationFormFieldDefinition(
            request.EvaluationFormId,
            request.Label,
            request.Description,
            fieldType,
            request.IsRequired,
            request.Order,
            request.Placeholder,
            request.OptionsJson,
            request.IsReadonly,
            request.DefaultValue,
            request.AllowedFileTypes,
            request.MaxFileSizeMb
            );
        evaluationFormFieldDefinitionRepository.Add(evaluationFormFieldDefinition);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed(evaluationFormFieldDefinition.Id.ToString());
    }
}

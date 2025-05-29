using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.Evaluations;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.EvaluationFormFields;
public sealed record UpdateFormFieldByEvaluationFormCommand(
    Guid Id,
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
        if (!Enum.IsDefined(typeof(FieldTypeEnum), request.Type))
        {
            return Result<string>.Failure(400,$"Invalid FielType: {request.Type}.");
        }
        var fieldType = (FieldTypeEnum)request.Type;

        evaluationFormFieldDefinition.Update(
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

        evaluationFormFieldDefinitionRepository.Update(evaluationFormFieldDefinition);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Form field updated successfully.");
    }
}

using BasvuruSistemi.Server.Domain.ApplicationFormTemplates;
using BasvuruSistemi.Server.Domain.FormFieldDefinitions;
using BasvuruSistemi.Server.Domain.JobPostings;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.FormTemplates;
public sealed record DeleteFormTemplateCommand(
    Guid Id
    ) : IRequest<Result<string>>;

internal sealed class DeleteFormTemplateCommandHandler(
    IFormTemplateRepository formTemplateRepository,
    IFormFieldDefinitionRepository formFieldDefinitionRepository,
    IUnitOfWork unitOfWork,
    IJobPostingRepository jobPostingRepository
    ) : IRequestHandler<DeleteFormTemplateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteFormTemplateCommand request, CancellationToken cancellationToken)
    {
        var isJobPostingExist = await jobPostingRepository.AnyAsync(p => p.FormTemplateId == request.Id && !p.IsDeleted);
        if (isJobPostingExist)
            return Result<string>.Failure(400, "This form template is cureently using on a jobPosting delete jobPosting before");

        var formTemplate = await formTemplateRepository
            .Where(p => p.Id == request.Id)
            .Include(p => p.FieldDefinitions)
            .FirstOrDefaultAsync(cancellationToken);

        if (formTemplate is null)
            return Result<string>.Failure(404, "Form template not found");

        formTemplate.IsDeleted = true;
        formTemplateRepository.Update(formTemplate);

        foreach(var fieldDefinition in formTemplate.FieldDefinitions)
        {
            fieldDefinition.IsDeleted = true;
            formFieldDefinitionRepository.Update(fieldDefinition);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Form Template deleted successfully");
    }
}

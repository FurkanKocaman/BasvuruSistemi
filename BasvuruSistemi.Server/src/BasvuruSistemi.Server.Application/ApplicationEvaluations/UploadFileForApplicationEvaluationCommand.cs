using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Evaluations;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using TS.Result;

namespace BasvuruSistemi.Server.Application.ApplicationEvaluations;
public sealed record UploadFileForApplicationEvaluationCommand(
    Guid formFieldId,
    IFormFile file,
    Guid evaluationPipelineStageId
    ) : IRequest<Result<string>>;

internal sealed class UploadFileForApplicationEvaluationCommandHandler(
    IWebHostEnvironment webHostEnvironment,
    ICurrentUserService currentUserService,
    IEvaluationFormFieldDefinitionRepository evaluationFormFieldDefinitionRepository
    ) : IRequestHandler<UploadFileForApplicationEvaluationCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UploadFileForApplicationEvaluationCommand request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        if (!userId.HasValue)
            return Result<string>.Failure("User not found");

        var field = await evaluationFormFieldDefinitionRepository.FirstOrDefaultAsync(p => p.Id == request.formFieldId);

        if (field is null)
            return Result<string>.Failure("Field not found");

        if (request.file == null || request.file.Length == 0)
            return Result<string>.Failure("Dosya boş olamaz.");

        var allowedExtensions = field.AllowedFileTypes!.Split(",").Select(p => p.Trim().ToLowerInvariant());
        var extension = Path.GetExtension(request.file.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(extension))
            return Result<string>.Failure("İnvalid file extension.");

        var wwwrootPath = webHostEnvironment.WebRootPath;
        if (!Directory.Exists(wwwrootPath))
            Directory.CreateDirectory(wwwrootPath);

        var uploadsRoot = Path.Combine(webHostEnvironment.WebRootPath, "uploads", "evaluation_files", request.evaluationPipelineStageId.ToString(), userId.Value.ToString());

        if (!Directory.Exists(uploadsRoot))
            Directory.CreateDirectory(uploadsRoot);

        var uniqueFileName = $"{Guid.NewGuid()}{request.file.FileName}";
        var filePath = Path.Combine(uploadsRoot, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await request.file.CopyToAsync(stream, cancellationToken);
        }

        var relativePath = $"/uploads/evaluation_files/{request.evaluationPipelineStageId.ToString()}/{userId.Value.ToString()}/{uniqueFileName}";
        return Result<string>.Succeed(relativePath);
    }
}

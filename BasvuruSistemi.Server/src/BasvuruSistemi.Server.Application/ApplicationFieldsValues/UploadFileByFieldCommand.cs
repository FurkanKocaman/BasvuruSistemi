using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.FormFieldDefinitions;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using TS.Result;

namespace BasvuruSistemi.Server.Application.ApplicationFieldsValues;
public sealed record UploadFileByFieldCommand(
    Guid formFieldId,
    IFormFile file
    ) : IRequest<Result<string>>;

internal sealed class UploadFileByFieldCommandhandler(
    IWebHostEnvironment webHostEnvironment,
    ICurrentUserService currentUserService,
    IFormFieldDefinitionRepository formFieldDefinitionRepository
    ) : IRequestHandler<UploadFileByFieldCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UploadFileByFieldCommand request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        if(!userId.HasValue)
            return Result<string>.Failure("User not found");

        var field = await formFieldDefinitionRepository.FirstOrDefaultAsync(p => p.Id == request.formFieldId);

        if (field is null)
            return Result<string>.Failure("Field not found");

        if (request.file == null || request.file.Length == 0)
            return Result<string>.Failure("Dosya boş olamaz.");

        var allowedExtensions = field.AllowedFileTypes!.Split(",").Select(p => p.Trim().ToLowerInvariant());
        var extension = Path.GetExtension(request.file.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(extension))
            return Result<string>.Failure("İnvalid file extension.");

        var uploadsRoot = Path.Combine(webHostEnvironment.WebRootPath, "uploads", "application_files", userId.Value.ToString());

        if (!Directory.Exists(uploadsRoot))
            Directory.CreateDirectory(uploadsRoot);

        var uniqueFileName = $"{Guid.NewGuid()}{request.file.FileName}";
        var filePath = Path.Combine(uploadsRoot, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await request.file.CopyToAsync(stream, cancellationToken);
        }

        var relativePath = $"/uploads/application_files/{userId.Value.ToString()}/{uniqueFileName}";
        return Result<string>.Succeed(relativePath);
    }
}

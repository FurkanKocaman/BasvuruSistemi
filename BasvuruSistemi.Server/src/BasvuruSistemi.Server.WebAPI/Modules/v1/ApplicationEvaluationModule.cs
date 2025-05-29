using BasvuruSistemi.Server.Application.ApplicationEvaluations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TS.Result;

namespace BasvuruSistemi.Server.WebAPI.Modules.v1;

public static class ApplicationEvaluationModule
{
    public static void RegisterApplicationEvaluationRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("v1/application-evaluations").WithTags("ApplicationEvaluations");

        group.MapPost("",
            async (ISender sender, SubmitEvaluationCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
            })
            .RequireAuthorization().Produces<Result<string>>();

        group.MapPost("upload-file/{formFieldId:guid}",
          async (ISender sender, Guid formFieldId, [FromQuery] Guid applicationEvaluationId, IFormFile file, CancellationToken cancellationToken) =>
          {
              var command = new UploadFileForApplicationEvaluationCommand(formFieldId, file, applicationEvaluationId);
              var response = await sender.Send(command, cancellationToken);
              return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
          })
       .DisableAntiforgery()
       .Accepts<IFormFile>("multipart/form-data")
       .RequireAuthorization()
       .Produces<Result<string>>();
    }
}

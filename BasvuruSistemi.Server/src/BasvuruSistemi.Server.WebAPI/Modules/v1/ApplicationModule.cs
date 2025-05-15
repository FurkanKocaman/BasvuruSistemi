using Azure;
using BasvuruSistemi.Server.Application.ApplicationFieldsValues;
using BasvuruSistemi.Server.Application.Applications;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TS.Result;

namespace BasvuruSistemi.Server.WebAPI.Modules.v1;

public static class ApplicationModule
{
    public static void RegisterApplicationRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("v1/applications").WithTags("Applications");

        group.MapPost("",
            async (ISender sender, ApplicationCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
            })
            .RequireAuthorization().Produces<Result<string>>();

        group.MapPut("{id}",
            async (ISender sender,Guid id, ApplicationUpdateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
            })
            .RequireAuthorization().Produces<Result<string>>();

        group.MapDelete("{id}",
           async (ISender sender, Guid id,CancellationToken cancellationToken) =>
           {
               var request = new ApplicationDeleteCommand(id);
               var response = await sender.Send(request, cancellationToken);
               return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
           })
           .RequireAuthorization().Produces<Result<string>>();

        group.MapPost("upload-file/{formFieldId:guid}",
           async (ISender sender, Guid formFieldId,[FromQuery]Guid applicationId, IFormFile file, CancellationToken cancellationToken) =>
           {
               var command = new UploadFileByFieldCommand(formFieldId, file,applicationId);
               var response = await sender.Send(command, cancellationToken);
               return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
           })
        .DisableAntiforgery()
        .Accepts<IFormFile>("multipart/form-data")
        .RequireAuthorization()
        .Produces<Result<string>>();
        group.MapPatch("withdrawn/{id}",
        async (ISender sender, Guid id, CancellationToken cancellationToken) =>
        {
            var request = new ApplicationWithdrawnCommand(id);
            var response = await sender.Send(request, cancellationToken);
            return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
        })
            .RequireAuthorization().Produces<Result<string>>();

        group.MapPatch("review/{id}",
           async (ISender sender, Guid id, ApplicationReviewCommand request, CancellationToken cancellationToken) =>
           {
               if(id != request.id)
                return Results.BadRequest("Id in the route and body must be same.");

               var response = await sender.Send(request, cancellationToken);
               return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
           })
           .RequireAuthorization().Produces<Result<string>>();
    }
}

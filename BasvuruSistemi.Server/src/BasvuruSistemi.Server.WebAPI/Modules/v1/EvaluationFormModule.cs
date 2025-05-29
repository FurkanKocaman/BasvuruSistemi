using BasvuruSistemi.Server.Application.EvaluationFormFields;
using BasvuruSistemi.Server.Application.EvaluationForms;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.WebAPI.Modules.v1;

public static class EvaluationFormModule
{
    public static void RegisterEvaluationFormRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("v1/evaluation-forms").WithTags("EvaluationForms");

        group.MapPost("",
            async (ISender sender, CreateEvaluationFormCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
            })
            .RequireAuthorization().Produces<Result<string>>().WithName("EvaluationFormCreate");

        group.MapPut("{id:guid}",
           async (ISender sender, Guid id, UpdateEvaluationFormCommand request, CancellationToken cancellationToken) =>
           {
               if (id != request.Id)
                   return Results.BadRequest("Id is not match");

               var response = await sender.Send(request, cancellationToken);
               return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
           })
           .RequireAuthorization().Produces<Result<string>>().WithName("EvaluationFormUpdate");

        group.MapDelete("{id:guid}",
            async (ISender sender, Guid id, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(new DeleteEvaluationFormCommand(id), cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
            })
            .RequireAuthorization().Produces<Result<string>>().WithName("EvaluationFormDelete");

        group.MapPost("fields",
           async (ISender sender, AddFormFieldToEvaluationFormCommand request, CancellationToken cancellationToken) =>
           {
               var response = await sender.Send(request, cancellationToken);
               return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
           })
           .RequireAuthorization().Produces<Result<string>>().WithName("EvaluationFormFieldCreate");

        group.MapPut("fields/{id:guid}",
          async (ISender sender, Guid id, UpdateFormFieldByEvaluationFormCommand request, CancellationToken cancellationToken) =>
          {
                if (id != request.Id)
                    return Results.BadRequest("ID in route does not match ID in body.");

              var response = await sender.Send(request, cancellationToken);
              return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
          })
          .RequireAuthorization().Produces<Result<string>>().WithName("EvaluationFormFieldUpdate");

        group.MapDelete("fields/{id}",
          async (ISender sender, Guid id, CancellationToken cancellationToken) =>
          {
              var response = await sender.Send(new DeleteFormFieldFromEvaluationFormCommand(id), cancellationToken);
              return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
          })
          .RequireAuthorization().Produces<Result<string>>().WithName("EvaluationFormFieldDelete");
    }
}

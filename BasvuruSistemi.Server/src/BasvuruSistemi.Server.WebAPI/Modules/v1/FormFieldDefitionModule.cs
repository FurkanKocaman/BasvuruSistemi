using BasvuruSistemi.Server.Application.FormFieldDefinitions;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.WebAPI.Modules.v1;

public static class FormFieldDefitionModule
{
    public static void RegisterFormFieldDefinitionRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("v1/form-field-definitions").WithTags("FormFieldDefinitions");

        group.MapPost("",
            async (ISender sender, FormFieldDefinitonCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>();

        group.MapPost("bulk",
            async (ISender sender, FormFieldDefinitonBulkCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>();
    }
}

using BasvuruSistemi.Server.Application.FormTemplates;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.WebAPI.Modules.v1;

public static class FormTemplateModule
{
    public static void RegisterFormTemplateRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("v1/form-templates").WithTags("FormTemplates");

        group.MapPost("",
            async (ISender sender, FormTemplateCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>();
    }
}

using BasvuruSistemi.Server.Application.Candidates;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.WebAPI.Modules;

public static class CandidateModule
{
    public static void RegisterCandidateRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/candidates").WithTags("Candidates");

        group.MapPost("/",
            async (ISender sender, CandidateCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>();
    }
}

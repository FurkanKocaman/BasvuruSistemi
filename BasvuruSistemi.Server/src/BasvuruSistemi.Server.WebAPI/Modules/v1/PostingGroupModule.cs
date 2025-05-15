using BasvuruSistemi.Server.Application.PostingGroups;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.WebAPI.Modules.v1;

public static class PostingGroupModule
{
    public static void RegisterPostingGroupRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("v1/api/posting-groups").WithTags("PostingGroups");
        group.MapPost("",
            async (ISender sender, PostingGroupCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .RequireAuthorization().Produces<Result<string>>();

        group.MapPost("{id:guid}/postings",
           async (ISender sender, Guid id, AddJobPostingToGroupCommand request, CancellationToken cancellationToken) =>
           {
               var command = request with { postingGroupId = id };
               var response = await sender.Send(command, cancellationToken);
               return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response): Results.InternalServerError(response);
           })
           .RequireAuthorization().Produces<Result<string>>();
    }
}

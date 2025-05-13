using BasvuruSistemi.Server.Application.JobPostings;
using BasvuruSistemi.Server.Domain.DTOs;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.WebAPI.Modules.v1;

public static class JobPostingModule
{
    public static void RegisterJobPostingRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("v1/api/job-postings").WithTags("JobPostings");

        group.MapPost("",
            async (ISender sender, JobPostingCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .RequireAuthorization().Produces<Result<string>>();

        group.MapPatch("{id:guid}/status",
           async (ISender sender, Guid id, ChangeJobPostingStatusDto dto, CancellationToken cancellationToken) =>
           {
               var request = new ChangeJobPostingStatusCommand(id, dto.NewStatus, dto.PublishStartDate);
               var response = await sender.Send(request, cancellationToken);
               return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
           })
           .RequireAuthorization().Produces<Result<string>>();

        group.MapPatch("{id:guid}",
           async (ISender sender, Guid id, JobPostingUpdateCommand request, CancellationToken cancellationToken) =>
           {
               var command = request with { id = id };
               var response = await sender.Send(command, cancellationToken);
               return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
           })
           .RequireAuthorization().Produces<Result<string>>();

    }
}

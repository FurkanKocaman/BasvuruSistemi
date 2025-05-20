using BasvuruSistemi.Server.Application.Addresses;
using BasvuruSistemi.Server.Application.Entities;
using BasvuruSistemi.Server.Application.Users;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.WebAPI.Modules.v1;

public static class UserModule
{
    public static void RegisterUserRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("v1/users").WithTags("Users");

        group.MapPut("avatar",
          async (ISender sender, UserUpdateProfileImageCommand request, CancellationToken cancellationToken) =>
          {
              var response = await sender.Send(request, cancellationToken);
              return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
          })
          .RequireAuthorization().Produces<Result<string>>();

        group.MapPost("education",
          async (ISender sender, EducationCreateCommand request, CancellationToken cancellationToken) =>
          {
              var response = await sender.Send(request, cancellationToken);
              return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
          })
          .RequireAuthorization().Produces<Result<string>>();

        group.MapDelete("education/{id}",
         async (ISender sender, Guid id,CancellationToken cancellationToken) =>
         {
             var request = new EducationDeleteCommand(id);
             var response = await sender.Send(request, cancellationToken);
             return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
         })
         .RequireAuthorization().Produces<Result<string>>();

        group.MapPost("experience",
          async (ISender sender, ExperienceCreateCommand request, CancellationToken cancellationToken) =>
          {
              var response = await sender.Send(request, cancellationToken);
              return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
          })
          .RequireAuthorization().Produces<Result<string>>();

        group.MapDelete("experience/{id}",
        async (ISender sender, Guid id, CancellationToken cancellationToken) =>
        {
            var request = new ExperienceDeleteCommand(id);
            var response = await sender.Send(request, cancellationToken);
            return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
        })
        .RequireAuthorization().Produces<Result<string>>();

        group.MapPost("skill",
          async (ISender sender, SkillCreateCommand request, CancellationToken cancellationToken) =>
          {
              var response = await sender.Send(request, cancellationToken);
              return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
          })
          .RequireAuthorization().Produces<Result<string>>();


        group.MapDelete("skill/{id}",
          async (ISender sender, Guid id, CancellationToken cancellationToken) =>
          {
              var request = new SkillDeleteCommand(id);
              var response = await sender.Send(request, cancellationToken);
              return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
          })
          .RequireAuthorization().Produces<Result<string>>();

        group.MapPost("certification",
          async (ISender sender, CertificationCreateCommand request, CancellationToken cancellationToken) =>
          {
              var response = await sender.Send(request, cancellationToken);
              return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
          })
          .RequireAuthorization().Produces<Result<string>>();

        group.MapDelete("certification/{id}",
          async (ISender sender, Guid id, CancellationToken cancellationToken) =>
          {
              var request = new CertificationDeleteCommand(id);
              var response = await sender.Send(request, cancellationToken);
              return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
          })
          .RequireAuthorization().Produces<Result<string>>();

        group.MapPost("address",
           async (ISender sender, AddressCreateCommand request, CancellationToken cancellationToken) =>
           {
               var response = await sender.Send(request, cancellationToken);
               return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
           })
           .RequireAuthorization().Produces<Result<string>>();

        group.MapPut("address/{id}",
           async (ISender sender,Guid id, UserAddressUpdateCommand request, CancellationToken cancellationToken) =>
           {
               if(id != request.UserId)
                   return Results.BadRequest("Id mismatch");
               var response = await sender.Send(request, cancellationToken);
               return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
           })
           .RequireAuthorization().Produces<Result<string>>();

    }
}

using Azure.Core;
using BasvuruSistemi.Server.Application.Addresses;
using BasvuruSistemi.Server.Application.Entities;
using BasvuruSistemi.Server.Application.Users;
using BasvuruSistemi.Server.Domain.DTOs;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.WebAPI.Modules.v1;

public static class UserModule
{
    public static void RegisterUserRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("v1/users").WithTags("Users");

        group.MapPut("{id:guid}",
          async (ISender sender, Guid id, UserUpdateCommand request, CancellationToken cancellationToken) =>
          {
              if(id != request.Id)
                  return Results.BadRequest("Id mismatch");
              var response = await sender.Send(request, cancellationToken);
              return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
          })
          .RequireAuthorization().Produces<Result<string>>();

        group.MapPut("avatar",
          async (ISender sender,IFormFile file, CancellationToken cancellationToken) =>
          {
              var response = await sender.Send(new UserUpdateProfileImageCommand(file), cancellationToken);
              return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
          })
          .DisableAntiforgery().RequireAuthorization().Produces<Result<string>>();

        group.MapPost("education",
          async (ISender sender, EducationCreateCommand request, CancellationToken cancellationToken) =>
          {
              var response = await sender.Send(request, cancellationToken);
              return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
          })
          .RequireAuthorization().Produces<Result<EducationDto>>();

        group.MapPut("education/{id:guid}",
        async (ISender sender, Guid id, EducationUpdateCommand request, CancellationToken cancellationToken) =>
        {
            if(id != request.Education.Id)
                return Results.BadRequest("Id mismatch");
            var response = await sender.Send(request, cancellationToken);
            return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
        })
        .RequireAuthorization().Produces<Result<string>>();

        group.MapDelete("education/{id:guid}",
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
          .RequireAuthorization().Produces<Result<ExperienceDto>>();

        group.MapPut("experience/{id:guid}",
          async (ISender sender, Guid id, ExperienceUpdateCommand request, CancellationToken cancellationToken) =>
          {
              if(id != request.Experience.Id)
                  return Results.BadRequest("Id mismatch");
              var response = await sender.Send(request, cancellationToken);
              return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
          })
          .RequireAuthorization().Produces<Result<string>>();

        group.MapDelete("experience/{id:guid}",
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
          .RequireAuthorization().Produces<Result<SkillDto>>();

        group.MapPut("skill/{id:guid}",
          async (ISender sender, Guid id, SkillUpdateCommand request, CancellationToken cancellationToken) =>
          {
              if (id != request.Skill.Id)
                  return Results.BadRequest("Id mismatch");
              var response = await sender.Send(request, cancellationToken);
              return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
          })
          .RequireAuthorization().Produces<Result<string>>();


        group.MapDelete("skill/{id:guid}",
          async (ISender sender, Guid id, CancellationToken cancellationToken) =>
          {
              var request = new SkillDeleteCommand(id);
              var response = await sender.Send(request, cancellationToken);
              return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
          })
          .RequireAuthorization().Produces<Result<string>>();



        group.MapPost("certificates",
          async (ISender sender, CertificationCreateCommand request, CancellationToken cancellationToken) =>
          {
              var response = await sender.Send(request, cancellationToken);
              return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
          })
          .RequireAuthorization().Produces<Result<CertificationDto>>();

        group.MapPut("certificates/{id:guid}",
          async (ISender sender, Guid id, CertificateUpdateCommand request, CancellationToken cancellationToken) =>
          {
              if (id != request.Certificate.Id)
                  return Results.BadRequest("Id mismatch");
              var response = await sender.Send(request, cancellationToken);
              return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
          })
          .RequireAuthorization().Produces<Result<string>>();

        group.MapDelete("certificates/{id:guid}",
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
           .RequireAuthorization().Produces<Result<AddressDto>>();

        group.MapPut("address/{id}",
           async (ISender sender,Guid id, UserAddressUpdateCommand request, CancellationToken cancellationToken) =>
           {
               if(id != request.Id)
                   return Results.BadRequest("Id mismatch");
               var response = await sender.Send(request, cancellationToken);
               return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
           })
           .RequireAuthorization().Produces<Result<AddressDto>>();

        group.MapDelete("address/{id}",
          async (ISender sender, Guid id,CancellationToken cancellationToken) =>
          {
              var response = await sender.Send(new AddressDeleteCommand(id), cancellationToken);
              return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
          })
          .RequireAuthorization().Produces<Result<string>>();

    }
}

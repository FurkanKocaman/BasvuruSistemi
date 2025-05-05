var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.BasvuruSistemi_Server_WebAPI>("BasvuruSistemi-webapi");

builder.Build().Run();

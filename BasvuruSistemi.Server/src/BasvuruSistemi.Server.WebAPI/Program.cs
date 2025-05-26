using BasvuruSistemi.Server.Application;
using BasvuruSistemi.Server.Infrastructure;
using BasvuruSistemi.Server.WebAPI;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.RateLimiting;
using Scalar.AspNetCore;
using System.Threading.RateLimiting;
using BasvuruSistemi.Server.WebAPI.Controllers.v1;
using BasvuruSistemi.Server.WebAPI.Modules;
using Microsoft.AspNetCore.Authentication;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddResponseCompression(opt =>
{
    opt.EnableForHttps = true;
});

builder.AddServiceDefaults();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddCors();
builder.Services.AddOpenApi();
builder.Services.AddControllers()
    .AddOData(opt =>
        opt
        .Select()
        .Filter()
        .Count()
        .Expand()
        .OrderBy()
        .SetMaxTop(null)
        .AddRouteComponents("odata", AppODataController.GetEdmModel()))
    ;
builder.Services.AddRateLimiter(x=>
x.AddFixedWindowLimiter("fixed",cfg =>
{
    cfg.QueueLimit = 100;
    cfg.Window = TimeSpan.FromSeconds(1);
    cfg.PermitLimit = 100;
    cfg.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
}));

builder.Services.AddExceptionHandler<ExceptionHandler>().AddProblemDetails();

builder.Services.AddTransient<IClaimsTransformation, CustomClaimsTransformation>();

//builder.Services.AddHangfire(config =>
//{
//    config.UseDashboardPath("/hangfire");
//});

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

app.UseHangfireDashboard("/hangfire");

app.MapDefaultEndpoints();

app.UseHttpsRedirection();

app.UseCors(x=> x.AllowAnyHeader().AllowCredentials().AllowAnyMethod().SetIsOriginAllowed(t=>true));

app.RegisterRoutes();

app.UseAuthentication();
app.UseAuthorization();

app.UseResponseCompression();

app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        // 365 gün
        ctx.Context.Response.Headers["Cache-Control"] = "public,max-age=31536000,immutable";
    }
});

app.UseExceptionHandler();

app.MapControllers().RequireRateLimiting("fixed").RequireAuthorization();

//ExtensionsMiddleware.CreateFirstUser(app);

app.MapPost("/schedule", (DateTime runAt, IBackgroundJobClient client) =>
{
    string jobId = client.Schedule(
        () => Console.WriteLine($"Job çalýþtý: {DateTime.Now}"),
        runAt - DateTime.Now
    );
    return Results.Ok(new { jobId });
});

app.Run();

using BasvuruSistemi.Server.WebAPI.Modules.v1;

namespace BasvuruSistemi.Server.WebAPI.Modules;

public static class RouteRegistrar
{
    public static void RegisterRoutes(this IEndpointRouteBuilder app)
    {
        app.RegisterAuthRoutes();
        app.RegisterTenantRoutes();
        app.RegisterUserRoutes();
        app.RegisterFormFieldDefinitionRoutes();
        app.RegisterFormTemplateRoutes();
        app.RegisterJobPostingRoutes();
        app.RegisterApplicationRoutes();
        app.RegisterUnitRoutes();
        app.RegisterPostingGroupRoutes();
    }
}

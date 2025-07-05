using System.Security.Claims;
using Fullstack.SAXS.Application;
using Fullstack.SAXS.Application.Contracts;
using Fullstack.SAXS.Domain.Contracts;
using Fullstack.SAXS.Infrastructure.DbContexts;
using Fullstack.SAXS.Infrastructure.Factories;
using Fullstack.SAXS.Infrastructure.Repositories;
using Fullstack.SAXS.Persistence.HTML;
using Fullstack.SAXS.Persistence.IO;
using Fullstack.SAXS.Server.Middlewares;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSingleton<ParticleFactory, IcosahedronFactory>()
    .AddSingleton<ParticleFactory, C60Factory>()
    .AddSingleton<IParticleFactoryResolver, ParticleFactoryResolver>()
    .AddScoped<ISysService, SysService>()
    .AddScoped<ISpService, SpService>()
    .AddScoped<IStorage, AreaRepository>()
    .AddScoped<IFileService, FileService>()
    .AddScoped<SysService>()
    .AddScoped<AreaFactory, SphereFactory>()
    .AddScoped<IGraphService, GraphService>()
    .AddSingleton<IStringService, StringService>();

builder.Services
    .AddDbContext<PosgresDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")))
    .AddDefaultIdentity<IdentityUser>()
    .AddEntityFrameworkStores<PosgresDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

builder.Services
    .AddControllersWithViews()
    .AddNewtonsoftJson(options => 
        options
        .SerializerSettings
        .ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
    );

builder.Services.AddRazorPages();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = context =>
    {
        if (context.Request.Path.StartsWithSegments("/api") || 
            context.Request.Path.StartsWithSegments("/ping-auth")
        )
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        }

        context.Response.Redirect(context.RedirectUri);
        return Task.CompletedTask;
    };

    options.Events.OnRedirectToAccessDenied = context =>
    {
        if (context.Request.Path.StartsWithSegments("/api") ||
            context.Request.Path.StartsWithSegments("/ping-auth")
        )
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        }

        context.Response.Redirect(context.RedirectUri);
        return Task.CompletedTask;
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseRouting();

app.MapGet("/ping-auth", (ClaimsPrincipal user) =>
{
    var email = user.FindFirstValue(ClaimTypes.Email);

    return Results.Json(new { Email = email });
}
).RequireAuthorization();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("/index.html");

app.Run();
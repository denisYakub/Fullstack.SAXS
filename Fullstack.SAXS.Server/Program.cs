using System.Security.Claims;
using Fullstack.SAXS.Application;
using Fullstack.SAXS.Application.Commands;
using Fullstack.SAXS.Application.Contracts;
using Fullstack.SAXS.Application.Services;
using Fullstack.SAXS.Infrastructure;
using Fullstack.SAXS.Persistence;
using Fullstack.SAXS.Persistence.DbContexts;
using Fullstack.SAXS.Persistence.DbListnres;
using Fullstack.SAXS.Persistence.Factories;
using Fullstack.SAXS.Persistence.Repositories;
using Fullstack.SAXS.Server.Middlewares;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSingleton<AreaFactory, SphereAreaFactory>()
    .AddSingleton<ParticleFactory, IcosahedronParticleFactory>()
    .AddSingleton<ParticleFactory, C60Factory>()
    .AddSingleton<ParticleFactory, C70Factory>()
    .AddSingleton<ParticleFactory, C240Factory>()
    .AddSingleton<ParticleFactory, C540Factory>()
    .AddSingleton<IParticleFactoryResolver, ParticleFactoryResolver>()
    .AddScoped<IDensityService, DensityService>()
    .AddScoped<IIntenceService, IntenceService>()
    .AddScoped<ISysService, SysService>()
    .AddScoped<IStorage, AreaRepository>();

builder.Services
    .AddFileService(builder.Configuration.GetSection("Csv"));

builder.Services
    .AddGraphService(builder.Configuration.GetSection("Graph"));

builder.Services
    .AddProducer<string>(builder.Configuration.GetSection("Kafka:SystemCreate"));

builder.Services
    .AddConsumer<string, CreateAreaHandler>(builder.Configuration.GetSection("Kafka:SystemCreate"));

builder.Services
    .AddHostedService<PostgresNotifyListener>();

builder.Services
    .AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(typeof(CreateTaskToCreateSystemHandler).Assembly);
    });

builder.Services
    .AddDbContext<PosgresDbContext>(
        options => options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"))
    )
    .AddDefaultIdentity<IdentityUser>()
    .AddEntityFrameworkStores<PosgresDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

builder.Services
    .AddControllersWithViews()
    .AddNewtonsoftJson();

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
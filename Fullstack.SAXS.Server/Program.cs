using System.Security.Claims;
using Fullstack.SAXS.Application;
using Fullstack.SAXS.Domain.Contracts;
using Fullstack.SAXS.Infrastructure.DbContexts;
using Fullstack.SAXS.Infrastructure.Factories;
using Fullstack.SAXS.Infrastructure.Repositories;
using Fullstack.SAXS.Persistence.HTML;
using Fullstack.SAXS.Persistence.IO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddScoped<IFileService, FileService>()
    .AddScoped<IStorage, AreaRepository>()
    .AddScoped<SysService>()
    .AddScoped<AreaParticleFactory, SphereIcosahedronFactory>()
    .AddScoped<IGraphService, GraphService>()
    .AddSingleton<IStringService, StringService>();

builder.Services
    .AddControllersWithViews()
    .AddNewtonsoftJson(options => 
        options
        .SerializerSettings
        .ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
    );

builder.Services.AddRazorPages();

builder.Services
    .AddDbContext<PosgresDbContext>(options => 
        options
        .UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"))
    )
    .AddDefaultIdentity<IdentityUser>()
    .AddEntityFrameworkStores<PosgresDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";

    options.Events.OnRedirectToLogin = context =>
    {
        if (context.Request.Path.StartsWithSegments("/ping-auth") && context.Response.StatusCode == 200)
        {
            context.Response.StatusCode = 401;
            return Task.CompletedTask;
        }

        context.Response.Redirect(context.RedirectUri);
        return Task.CompletedTask;
    };
});

builder.Services.AddAuthorization();

//builder.Services.AddOpenApi();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

//app.MapStaticAssets();

app.UseRouting();

app.MapGet("/ping-auth", (ClaimsPrincipal user) =>
{
    var email = user.FindFirstValue(ClaimTypes.Email);

    return Results.Json(new { Email = email });
}
).RequireAuthorization();

if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("/index.html");

app.Run();
using System.Security.Claims;
using Fullstack.SAXS.Server.Application.Interfaces;
using Fullstack.SAXS.Server.Domain.Entities.Areas;
using Fullstack.SAXS.Server.Domain.Entities.Octrees;
using Fullstack.SAXS.Server.Infastructure.Factories;
using Fullstack.SAXS.Server.Infastructure.Persistence.DbContexts;
using Fullstack.SAXS.Server.Infastructure.Persistence.Repositories;
using Fullstack.SAXS.Server.Infastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var csvFolder = 
    builder.Configuration["FilesFolder:CsvFolder"]
    ?? 
    Path.Combine(
        AppContext.BaseDirectory,
        "CsvResults"
    );

if (!Directory.Exists(csvFolder))
    Directory.CreateDirectory(csvFolder);

builder.Services
    .AddScoped<IFileService<Area>, FileService>()
    .AddScoped<ISysService, SysService>()
    .AddScoped<IRepository<Area>, AreaRepository>()
    .AddScoped<AreaParticleFactory, SphereIcosahedronFactory>()
    .AddSingleton<string>(csvFolder);

builder.Services
    .AddControllers()
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

/*builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";

    options.Events.OnRedirectToLogin = context =>
    {
        if (context.Request.Path.StartsWithSegments("/ping-auth")
            && context.Request.Path.StartsWithSegments("/api/Main")
            && context.Request.Path.StartsWithSegments("/swagger/index.html")
            && context.Response.StatusCode == 200)
        {
            context.Response.StatusCode = 401;
            return Task.CompletedTask;
        }

        context.Response.Redirect(context.RedirectUri);
        return Task.CompletedTask;
    };
});*/

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

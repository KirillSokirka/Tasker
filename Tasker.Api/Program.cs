using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Tasker.Application.Commands;
using Tasker.Application.Interfaces;
using Tasker.Application.Interfaces.Commands;
using Tasker.Application.Interfaces.Queries;
using Tasker.Application.Interfaces.Repositories;
using Tasker.Application.Queries;
using Tasker.Application.Repositories;
using Tasker.Application.Services;
using Tasker.Domain;
using Tasker.Domain.Entities.Identity;
using Tasker.Infrastructure.Data.Application;
using Tasker.Infrastructure.Data.Identity;
using Tasker.Infrastructure.Data.Seed;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<IdentityContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<IdentityContext>()
    .AddDefaultTokenProviders();

builder.Services.AddTransient<IProjectRepository, ProjectRepository>();
builder.Services.AddTransient<IUserAuthService, UserAuthAuthService>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IFindUserByNameQuery, FindUserByNameQuery>();
builder.Services.AddTransient<IFindByIdQuery, FindUserByIdQuery>();
builder.Services.AddTransient<IUpdateUserCommand, UpdateUserCommand>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ??
                                throw new KeyNotFoundException("Key should be specified"))),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});
builder.Services.AddAuthorization();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var applicationContext = services.GetRequiredService<ApplicationContext>();

    if (applicationContext.Database.GetPendingMigrations().Any())
    {
        applicationContext.Database.Migrate();
    }

    var identityContext = services.GetRequiredService<IdentityContext>();

    if (identityContext.Database.GetPendingMigrations().Any())
    {
        identityContext.Database.Migrate();
    }

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();


    await IdentityDbSeeder.SeedRolesAsync(roleManager);
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Tasker.Application.Commands;
using Tasker.Application.Interfaces;
using Tasker.Application.Interfaces.Commands;
using Tasker.Application.Interfaces.Queries;
using Tasker.Application.Interfaces.Resolvers;
using Tasker.Application.Interfaces.Services;
using Tasker.Application.MappingProfiles;
using Tasker.Application.Queries;
using Tasker.Application.Resolvers;
using Tasker.Application.Services;
using Tasker.Domain.Entities.Application;
using Tasker.Domain.Entities.Identity;
using Tasker.Infrastructure.Data.Application;
using Tasker.Infrastructure.Data.Identity;
using Tasker.Infrastructure.Data.Seed;
using Tasker.Middleware;
using TaskStatus = Tasker.Domain.Entities.Application.TaskStatus;
using Task = Tasker.Domain.Entities.Application.Task;
using Tasker.Application.Resolver;
using Tasker.Application.Services.Application;
using Tasker.Domain.Repositories;
using Tasker.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<IdentityContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<IdentityContext>()
    .AddRoles<IdentityRole>()
    .AddDefaultTokenProviders();

// Commands and Queries
builder.Services.AddTransient<IFindUserByNameQuery, FindUserByNameQuery>();
builder.Services.AddTransient<IFindByIdQuery, FindUserByIdQuery>();
builder.Services.AddTransient<IGetUserRolesQuery, GetUserRolesQuery>();
builder.Services.AddTransient<IUpdateUserCommand, UpdateUserCommand>();

// Repositories
builder.Services.AddScoped<IEntityRepository<User>, UserRepository>();
builder.Services.AddScoped<IEntityRepository<Task>, TaskRepository>();
builder.Services.AddScoped<IEntityRepository<TaskStatus>, TaskStatusRepository>();
builder.Services.AddScoped<IEntityRepository<KanbanBoard>, KanbanBoardRepository>();
builder.Services.AddScoped<IEntityRepository<Project>, ProjectRepository>();
builder.Services.AddScoped<IEntityRepository<Release>, ReleaseRepository>();

// Services
builder.Services.AddTransient<IUserAuthService, UserAuthService>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ITaskStatusService, TaskStatusService>();
builder.Services.AddScoped<IReleaseService, ReleaseService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IKanbanBoardService, KanbanBoardService>();

// Resolvers
builder.Services.AddScoped<IUserResolver, UserResolver>();
builder.Services.AddScoped<ITaskResolver, TaskResolver>();
builder.Services.AddScoped<IProjectResolver, ProjectResolver>();
builder.Services.AddScoped<IResolver<Release, string>, ReleaseResolver>();
builder.Services.AddScoped<IResolver<KanbanBoard, string>, KanbanBoardResolver>();
builder.Services.AddScoped<IResolver<TaskStatus, string>, TaskStatusResolver>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddControllers();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

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
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlerMiddleware>();

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
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

    await IdentityDbSeeder.SeedRolesAsync(roleManager);
    await IdentityDbSeeder.SeedTestUserAsync(userManager);
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
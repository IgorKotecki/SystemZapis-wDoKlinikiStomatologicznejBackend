using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using ProjektSemestralnyTinWebApi.Security;
using SystemZapisowDoKlinikiApi.Controllers;
using SystemZapisowDoKlinikiApi.Middlewares;
using SystemZapisowDoKlinikiApi.Models;
using SystemZapisowDoKlinikiApi.Repositories;
using SystemZapisowDoKlinikiApi.Repositories.RepositoriesInterfaces;
using SystemZapisowDoKlinikiApi.Services;
using SystemZapisowDoKlinikiApi.Services.ServiceInterfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Repositories
builder.Services.AddScoped<ITimeBlockRepository, TimeBlockRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDoctorDaySchemeRepository, DoctorDaySchemeRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IToothRepository, ToothRepository>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<ICloudinaryRepository, CloudinaryRepository>();

// Services
builder.Services.AddScoped<ITimeBlockService, TimeBlockService>();
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDoctorDaySchemeService, DoctorDaySchemeService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IToothService, ToothService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();

builder.Services.AddSingleton<IEmailService, EmailSender>();

builder.Services.AddHostedService<DailyService>();

builder.Services.AddHttpContextAccessor();

builder.Logging.ClearProviders();
builder.Logging.AddOpenTelemetry(x =>
{
    x.IncludeScopes = true;
    x.IncludeFormattedMessage = true;

    x.AddOtlpExporter(a =>
    {
        a.Endpoint = new Uri(builder.Configuration["Otlp:Endpoint"]!);
        a.Protocol = OtlpExportProtocol.HttpProtobuf;
        a.Headers = builder.Configuration["Otlp:Headers"]!;
    });
});

builder.Logging.AddFilter(
    "Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware",
    LogLevel.None);

builder.Services.AddDbContext<ClinicDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            ),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy => policy.WithOrigins("http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

app.UseCors("AllowLocalhost");

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
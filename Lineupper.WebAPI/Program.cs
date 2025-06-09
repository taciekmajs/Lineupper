using FluentValidation;
using FluentValidation.AspNetCore;
using Lineupper.Application.Mappings;
using Lineupper.Application.Services.Implementations;
using Lineupper.Application.Services.Interfaces;
using Lineupper.Domain.Contracts;
using Lineupper.Infrastructure;
using Lineupper.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using Lineupper.Application.Validators;

var logger = LogManager.Setup()
    .LoadConfigurationFromFile("nlog.config")
    .GetCurrentClassLogger();

logger.Debug("Start aplikacji Lineupper.WebAPI");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // ⚙️ NLog
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // 🗄️ SQLite
    builder.Services.AddDbContext<LineupperDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

    // 📄 Swagger (OpenAPI)
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // 📦 Kontrolery
    builder.Services.AddControllers();

    // 🧪 FluentValidation
    builder.Services.AddFluentValidationAutoValidation();
    builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserDtoValidator>(); // rejestruje wszystkie walidatory z Application

    // 🔄 AutoMapper
    builder.Services.AddAutoMapper(typeof(MappingProfile));

    // 🔐 Dependency Injection: Serwisy
    builder.Services.AddScoped<IBandService, BandService>();
    builder.Services.AddScoped<IFestivalService, FestivalService>();
    builder.Services.AddScoped<IVoteService, VoteService>();
    builder.Services.AddScoped<IScheduleItemService, ScheduleItemService>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IParticipantService, ParticipantService>();
    builder.Services.AddScoped<IOrganizerService, OrganizerService>();

    // 📥 Dependency Injection: Repozytoria
    builder.Services.AddScoped<IBandRepository, BandRepository>();
    builder.Services.AddScoped<IFestivalRepository, FestivalRepository>();
    builder.Services.AddScoped<IVoteRepository, VoteRepository>();
    builder.Services.AddScoped<IScheduleItemRepository, ScheduleItemRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IParticipantRepository, ParticipantRepository>();
    builder.Services.AddScoped<IOrganizerRepository, OrganizerRepository>();

    // 🔄 Unit of Work
    builder.Services.AddScoped<IUnitOfWork, LineupperUnitOfWork>();

    // 🌐 CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowWasm", policy =>
        {
            policy.WithOrigins("https://localhost:7297")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
    });

    var app = builder.Build();

    app.UseCors("AllowWasm");

    // 🧪 Seeding bazy danych
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<LineupperDbContext>();
        DataSeeder.SeedDatabase(dbContext);
    }

    // 🧪 Swagger (tylko w dev)
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // ⚠️ Middleware do obsługi wyjątków
    app.UseMiddleware<Lineupper.WebAPI.Middleware.ExceptionMiddleware>();

    app.UseHttpsRedirection();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Błąd krytyczny przy uruchamianiu aplikacji");
    throw;
}
finally
{
    LogManager.Shutdown();
}

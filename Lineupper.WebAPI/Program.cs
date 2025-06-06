using Lineupper.Application.Mappings;
using Lineupper.Application.Services.Implementations;
using Lineupper.Application.Services.Interfaces;
using Lineupper.Domain.Contracts;
using Lineupper.Infrastructure;
using Lineupper.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Konfiguracja SQLite z appsettings.json
builder.Services.AddDbContext<LineupperDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Swagger (OpenAPI)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

// Serwisy
builder.Services.AddScoped<IBandService, BandService>();
builder.Services.AddScoped<IFestivalService, FestivalService>();
builder.Services.AddScoped<IVoteService, VoteService>();
builder.Services.AddScoped<IScheduleItemService, ScheduleItemService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IParticipantService, ParticipantService>();
builder.Services.AddScoped<IOrganizerService, OrganizerService>();

// Repozytoria
builder.Services.AddScoped<IBandRepository, BandRepository>();
builder.Services.AddScoped<IFestivalRepository, FestivalRepository>();
builder.Services.AddScoped<IVoteRepository, VoteRepository>();
builder.Services.AddScoped<IScheduleItemRepository, ScheduleItemRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IParticipantRepository, ParticipantRepository>();
builder.Services.AddScoped<IOrganizerRepository, OrganizerRepository>();

// UnitOfWork
builder.Services.AddScoped<IUnitOfWork, LineupperUnitOfWork>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWasm",
        builder => builder
            .WithOrigins("https://localhost:7297") 
            .AllowAnyHeader()
            .AllowAnyMethod());
});



var app = builder.Build();
app.UseCors("AllowWasm");


using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<LineupperDbContext>();
    DataSeeder.SeedDatabase(dbContext); 
}

// Środowisko developerskie – Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

// Middleware do obsługi np. błędów lub logowania możesz dodać później
// app.UseMiddleware<ErrorHandlingMiddleware>();



app.Run();

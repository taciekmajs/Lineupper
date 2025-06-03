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

// Tu w przyszłości zarejestrujesz swoje serwisy, repozytoria itd.
builder.Services.AddScoped<IUnitOfWork, LineupperUnitOfWork>();
builder.Services.AddScoped<IFestivalRepository, FestivalRepository>();
builder.Services.AddScoped<IBandRepository, BandRepository>();
builder.Services.AddScoped<IVoteRepository, VoteRepository>();
builder.Services.AddScoped<IScheduleItemRepository, ScheduleItemRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

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

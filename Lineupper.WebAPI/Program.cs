using Lineupper.Infrastructure; 
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Konfiguracja SQLite z appsettings.json
builder.Services.AddDbContext<LineupperDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Swagger (OpenAPI)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Tu w przysz³oœci zarejestrujesz swoje serwisy, repozytoria itd.
// builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<LineupperDbContext>();
    DataSeeder.SeedDatabase(dbContext); 
}

// Œrodowisko developerskie – Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Middleware do obs³ugi np. b³êdów lub logowania mo¿esz dodaæ póŸniej
// app.UseMiddleware<ErrorHandlingMiddleware>();

app.Run();

using Microsoft.EntityFrameworkCore;

using Entity;
using Business;
using Data;  // Asegúrate de tener esto para que reconozca ApplicationDbContext

var builder = WebApplication.CreateBuilder(args);

// Configurar Entity Framework con PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("Entity")));

builder.Services.AddControllers();
builder.Services.AddScoped<RolData>(); 
builder.Services.AddScoped<RolBusiness>();

builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddSwaggerGen();
var app = builder.Build();

//app.UseHttpsRedirection();
app.MapControllers();
app.Run();

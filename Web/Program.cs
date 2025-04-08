using Business;
using Data;
using Entity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<RolBusiness>();
builder.Services.AddScoped<RolData>();

builder.Services.AddScoped<FormBusiness>();
builder.Services.AddScoped<FormData>();

builder.Services.AddScoped<PersonBusiness>();
builder.Services.AddScoped<PersonData>();

builder.Services.AddScoped<PermissionBusiness>();
builder.Services.AddScoped<PermissionData>();

builder.Services.AddScoped<ModuleBusiness>();
builder.Services.AddScoped<ModuleData>();

builder.Services.AddScoped<UserBusiness>();
builder.Services.AddScoped<UserData>();

builder.Services.AddScoped<UserRolBusinness>();
builder.Services.AddScoped<UserRolData>();

builder.Services.AddScoped<ModuleFormBusiness>();
builder.Services.AddScoped<ModuleFormData>();

builder.Services.AddScoped<RolFormPermissionBusiness>();
builder.Services.AddScoped<RolFormPermissionData>();


builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// se define la pgAdmin => paquete
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//   options.UseMySQL(builder.Configuration.GetConnectionString("MySQL"))
//);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
   options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(); 
}

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web v1"));

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger");
        return;
    }
    await next();
});

app.UseAuthorization();
app.MapControllers();
app.Run();

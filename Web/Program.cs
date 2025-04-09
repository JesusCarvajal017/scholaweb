using Business;
using Business.services;
using Data;
using Data.factories;
using Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// db disponibles
//MySQL
//SQLServer
//PgAdmin

builder.Services.AddAuthorization();

builder.Services.AddScoped<IDataFactoryGlobal, GlobalFactory>();

builder.Services.AddDataAccessFactory("MySQL", builder.Configuration);
builder.Services.AddScoped<PersonBusiness>();
//builder.Services.AddScoped<UserBusiness>();
builder.Services.AddScoped<RolBusiness>();
builder.Services.AddScoped<FormBusiness>();
builder.Services.AddScoped<ModuleBusiness>();
builder.Services.AddScoped<ModuleFormBusiness>();
builder.Services.AddScoped<UserBusiness>();
builder.Services.AddScoped<UserRolBusiness>();
builder.Services.AddScoped<RolFormPermissionBusiness>();
builder.Services.AddScoped<PermissionBusiness>();



builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(Helper.MappingProfile));


var app = builder.Build();
  
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

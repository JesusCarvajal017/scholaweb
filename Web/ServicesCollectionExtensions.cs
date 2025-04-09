using Data.factories;
using Entity;
using Entity.Model;
using Microsoft.EntityFrameworkCore;

namespace Web
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddDataAccessFactory(this IServiceCollection services,string dbString, IConfiguration configuration)
        {
            // Leer la configuración del motor, por ejemplo, "Postgres", "Mysql", "SqlServer", etc.
            string databaseEngine = dbString;

            switch (databaseEngine)
            {
                case "PgAdmin":
                    //services.AddScoped<IDataAccessFactory, PostgrestDataFactory>();

                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseNpgsql(configuration.GetConnectionString(dbString)));


                    break;
                case "MySQL":
                    //services.AddScoped<IDataAccessFactory, MysqlDataFactory>();
                    //services.AddScoped<IDataAccessFactory, MySqlDataFactory>();
                     
                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseMySQL(configuration.GetConnectionString(dbString)));
                    break;
                case "SQLServer":

                    //services.AddScoped<IDataAccessFactory, SqlServerDataFactory>();

                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseSqlServer(configuration.GetConnectionString(dbString)));

                    break;
                default:
                    throw new InvalidOperationException("Motor de base de datos no soportado o no configurado.");
            }

            return services;
        }
    }
}

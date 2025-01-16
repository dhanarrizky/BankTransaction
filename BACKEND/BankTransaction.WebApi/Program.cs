using System.Net;
using BankTransaction.DataAccessAndBusiness;
using BankTransaction.WebApi.Configuration;
using BankTransaction.WebApi.MiddleWare;
using BankTransaction.WebApi.Services;

namespace TraBea.WebApi {
    public static class Program {
        public static void Main(string[] args){

            var builder = WebApplication.CreateBuilder(args);
             Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureKestrel((context, options) =>
                {
                    var port = context.Configuration.GetValue<int>("Kestr el:EndPoints:Http:Port");
                    options.Listen(IPAddress.Loopback, port);
                });
            });

            builder.Services.AddDataAccessAndBusiness();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowMyApp", builder =>
                {
                    builder.WithOrigins("http://localhost:5173")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<SqlDatabaseService>(sp => 
                new SqlDatabaseService(builder.Configuration.GetConnectionString("DefaultConnection") ?? "")
            );

            builder.Services.AddMemoryCache();
            builder.Services.AddScoped<MiddleWareServices>();
            builder.Services.AddScoped<TransactionServices>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.MapControllers();
            app.UseCors("AllowMyApp");
            app.Run();
        }
    }
}
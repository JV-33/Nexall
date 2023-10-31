using Microsoft.EntityFrameworkCore;
using Nexall.Data;
using Nexall.Data.DataContext;
using Nexall.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace NEXALL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Pievienot datu kontekstu un interfeisu
            builder.Services.AddDbContext<NexallContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<INexallContext, NexallContext>();

            // Pievienot servisus
            builder.Services.AddScoped<ICarStatisticsService, CarStatisticsService>();
            builder.Services.AddScoped<DataImportService>();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policyBuilder =>
                {
                    policyBuilder.WithOrigins("http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            // Ielādēt datus no speed.txt faila
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<INexallContext>();
                    var dataImportService = services.GetRequiredService<DataImportService>();
                    dataImportService.ImportData("../Nexall.Data/speed.txt");
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Kļūda, importējot datus.");
                }
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}

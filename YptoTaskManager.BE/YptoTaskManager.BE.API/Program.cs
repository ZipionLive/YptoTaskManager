using Microsoft.EntityFrameworkCore;
using YptoTaskManager.BE.Data;

namespace YptoTaskManager.BE.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<YptoTaskManagerDbContext>(options =>
            {
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    sql =>
                    {
                        sql.MigrationsAssembly("YptoTaskManager.BE.Migrations");
                    });
            });

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new()
                {
                    Title = "Ypto Task Manager API",
                    Version = "v1",
                    Description = "Task management system challenge"
                });
            });

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            // app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
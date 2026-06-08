using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;
using YptoTaskManager.BE.Business;
using YptoTaskManager.BE.Business.Security;
using YptoTaskManager.BE.Data;
using YptoTaskManager.BE.Data.Commands;
using YptoTaskManager.BE.Data.Queries;
using YptoTaskManager.BE.IBusiness;
using YptoTaskManager.BE.IBusiness.Security;
using YptoTaskManager.BE.IData;
using YptoTaskManager.BE.IData.Commands;
using YptoTaskManager.BE.IData.Queries;

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

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT token only. Do not prefix with Bearer."
                });
            });

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<ITaskItemQueryRepository, TaskItemQueryRepository>();
            builder.Services.AddScoped<ITaskItemStatusQueryRepository, TaskItemStatusQueryRepository>();
            builder.Services.AddScoped<ITaskItemTypeQueryRepository, TaskItemTypeQueryRepository>();
            builder.Services.AddScoped<IUserQueryRepository, UserQueryRepository>();

            builder.Services.AddScoped<ITaskItemCommandRepository, TaskItemCommandRepository>();
            builder.Services.AddScoped<IUserCommandRepository, UserCommandRepository>();

            builder.Services.AddScoped<ITaskItemService, TaskItemService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

            var jwtIssuer = builder.Configuration["Jwt:Issuer"]
                ?? throw new InvalidOperationException("Missing Jwt:Issuer");

            var jwtAudience = builder.Configuration["Jwt:Audience"]
                ?? throw new InvalidOperationException("Missing Jwt:Audience");

            var jwtKey = builder.Configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("Missing Jwt:Key");

            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtIssuer,

                        ValidateAudience = true,
                        ValidAudience = jwtAudience,

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtKey)),

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(2)
                    };
                });

            builder.Services.AddAuthorization();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Frontend", policy =>
                {
                    policy.WithOrigins("https://localhost:xxxx", "http://localhost:xxxx")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });


            var app = builder.Build();

            app.UseCors("Frontend");

            app.UseSwagger();
            app.UseSwaggerUI();

            // app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
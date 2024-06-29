
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;
using UserBLL.Interface;
using UserBLL.Service;
using UserRLL.Context;
using UserRLL.Interface;
using UserRLL.Services;
using UserRLL.Utilities;

namespace FundooWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Add Controllers 
            builder.Services.AddControllers();
            
            // Add DbContext
            builder.Services.AddDbContextPool<UserDBContext>(option => option.UseSqlServer(Environment.GetEnvironmentVariable("UserDbConnection")));

            // Add Email Service 
            builder.Services.AddTransient<EmailSender>();

            // Add Services
            builder.Services.AddScoped<IUserBL,UserBL>();
            builder.Services.AddScoped<IUserRL,UserRL>();

            builder.Services.AddScoped<INoteBL, NoteBL>();
            builder.Services.AddScoped<INoteRL ,NoteRL>();

            builder.Services.AddScoped<ILabelBL, LabelBL>();
            builder.Services.AddScoped<ILabelRL, LabelRL>();

            builder.Services.AddScoped<INoteLabelBL, NoteLabelBL>();
            builder.Services.AddScoped<INoteLabelRL, NoteLabelRL>();

            builder.Services.AddScoped<ICollaboratorBL, CollaboratorBL>();
            builder.Services.AddScoped<ICollaboratorRL, CollaboratorRL>();

            builder.Services.AddScoped<JwtTokenGenerator>();

            builder.Services.AddScoped<ICacheService, CacheService>();

            builder.Services.AddScoped<IRabitMQProducer, RabitMQProducer>();

            // CORS Policy
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.WithOrigins("https://learn.microsoft.com")  // Allow any origin for testing purposes
                                        .AllowAnyHeader()
                                        .AllowAnyMethod();
                                  });
            });

            // Add Appsettings Configuration Builder 
            builder.Configuration.SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            // Getting Values from AppSettings.json
            var config = builder.Configuration;
            var secretKey = Environment.GetEnvironmentVariable("SecretKey");
            var issuer = config["Jwt:ValidIssuer"];
            var audience = config["Jwt:ValidAudience"];

            // Redis Connection 
            // Configure Lazy<ConnectionMultiplexer>
            builder.Services.AddSingleton<Lazy<ConnectionMultiplexer>>(sp =>
            {
                var configurations = sp.GetRequiredService<IConfiguration>();
                return new Lazy<ConnectionMultiplexer>(() =>
                {
                    return ConnectionMultiplexer.Connect(configurations["RedisURL"] ?? throw new Exception("Provide Correct URL to Connect to Redis Server"));
                });
            });

            // AddStackExchangeRedisCache if using distributed cache
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration["RedisURL"];
                options.InstanceName = "SampleInstance";
            });



            // JWT Authentication service 
            builder.Services.AddAuthentication().AddJwtBearer("CrudScheme", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey ?? throw new Exception("Provide Secret Key")))
                };
            })
            .AddJwtBearer("UserValidationScheme", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey ?? throw new Exception("Provide Secret Key")))
                };
            })
            .AddJwtBearer("EmailVerificationScheme", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey ?? throw new Exception("Provide Secret Key")))
                };
            });

            // Session State -- Can be implemented using Distributed Caching(Redis)
            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = "_SessionKey";
                options.IdleTimeout = TimeSpan.FromSeconds(6000);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSession();

            app.MapControllers();

            app.Run();
        }
    }
}

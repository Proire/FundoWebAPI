
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
            builder.Services.AddTransient<IUserBL,UserBL>();
            builder.Services.AddTransient<IUserRL,UserRL>();

            builder.Services.AddTransient<INoteBL, NoteBL>();
            builder.Services.AddTransient<INoteRL ,NoteRL>();

            builder.Services.AddTransient<ILabelBL, LabelBL>();
            builder.Services.AddTransient<ILabelRL, LabelRL>();

            builder.Services.AddTransient<INoteLabelBL, NoteLabelBL>();
            builder.Services.AddTransient<INoteLabelRL, NoteLabelRL>();

            builder.Services.AddTransient<ICollaboratorBL, CollaboratorBL>();
            builder.Services.AddTransient<ICollaboratorRL, CollaboratorRL>();

            builder.Services.AddSingleton<JwtTokenGenerator>();

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

            // Getting Configuration object which now represents appsettings.json inside our program
            var configuration = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();

            // Getting Values from AppSettings.json
            var config = builder.Configuration;
            var secretKey = Environment.GetEnvironmentVariable("SecretKey");
            var issuer = config["Jwt:ValidIssuer"];
            var audience = config["Jwt:ValidAudience"];

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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
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

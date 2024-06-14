
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
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

            // Add Services
            builder.Services.AddTransient<IUserBL,UserBL>();
            builder.Services.AddTransient<IUserRL,UserRL>();

            builder.Services.AddTransient<INoteBL, NoteBL>();
            builder.Services.AddTransient<INoteRL ,NoteRL>();

            builder.Services.AddTransient<ILabelBL, LabelBL>();
            builder.Services.AddTransient<ILabelRL, LabelRL>();

            builder.Services.AddTransient<INoteLabelBL, NoteLabelBL>();
            builder.Services.AddTransient<INoteLabelRL, NoteLabelRL>(); 

            // Getting Configuration object which now represents appsettings.json inside our program
            var configuration = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();
            // JWT Authentication service 
            builder.Services.AddAuthentication( JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,options =>
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidIssuer = configuration["JWT:ValidIssuer"],
                ValidAudience = configuration["JWT:ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SecretKey") ?? string.Empty))
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
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

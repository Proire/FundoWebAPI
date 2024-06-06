
using Microsoft.EntityFrameworkCore;
using UserBLL.Interface;
using UserBLL.Service;
using UserRLL.Context;
using UserRLL.Interface;
using UserRLL.Services;

namespace FundooWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContextPool<UserDBContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("UserDBConnection")));
            builder.Services.AddTransient<IUserBL,UserBL>();
            builder.Services.AddTransient<IUserRL,UserRL>();
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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

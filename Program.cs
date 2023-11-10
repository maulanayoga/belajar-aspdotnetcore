using TestAPI.Context;
using Microsoft.EntityFrameworkCore;
using TestAPI.Repository;

namespace TestAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<MyContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("APIContext")));
            builder.Services.AddScoped<EmployeeRepository>();
            builder.Services.AddScoped<DepRepository>();
            builder.Services.AddScoped<ChartRepository>();
            
            builder.Services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            //app.UseCors(options => options.AllowAnyOrigin());

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
           

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
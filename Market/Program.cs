using Market.Hubs;
using Market.Interfaces;
using Market.Models;
using Market.Repository;
using Microsoft.EntityFrameworkCore;
using Market.utilities;

namespace Market
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var MyAllowSpecificOrigin = "_MyAllowSpecificOrigin";


            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddDbContext<StockMarketContext>(op =>
                op.UseSqlServer(builder.Configuration.GetConnectionString("Stockcon"))
                );
            builder.Services.AddScoped<IStockRepository, StockRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();


            //builder.Services.AddHostedService<StockPriceGenerator>();
            builder.Services.AddSignalR();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options=>
            {
                options.AddPolicy(MyAllowSpecificOrigin,
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200");
                        builder.AllowAnyMethod();
                        builder.AllowAnyHeader();
                        builder.AllowCredentials();
                    }
                );

            });

            var app = builder.Build();

            app.UseCors(MyAllowSpecificOrigin);
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }



            app.UseHttpsRedirection();


            app.MapControllers();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<StockHub>("/stockHub");
            });


            app.Run();
        }
    }
}
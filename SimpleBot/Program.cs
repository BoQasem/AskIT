using Microsoft.EntityFrameworkCore;
using AskIT.Models;
using AskIT.Services;
namespace AskIT
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors();

            builder.Services.AddDbContext<WhatsAppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("WhatsAppDB")));


            builder.Services.AddScoped<MessageMatcher>();
            builder.Services.AddScoped<MessageSender>();
            builder.Services.AddScoped<MessageLogger>();
            builder.Services.AddScoped<MessageProcessor>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}

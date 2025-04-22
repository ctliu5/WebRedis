using AiritiUtility.Session.Redis.Extensions;
using Microsoft.AspNetCore.Builder;
using ProtoBuf.Meta;
using WebRedis.Service;

namespace WebRedis
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddAiritiRedis<SessionRedis>(builder.Configuration.GetConnectionString("RedisConnection")!);

            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.MapRazorPages();

            app.Run();
        }
    }
}

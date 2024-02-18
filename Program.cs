using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Lesson1.Data;
using Microsoft.Identity.Client;
using Lesson1.Models;
namespace Lesson1
{
    public class Program
    {
        private static User? _user;
        public static User? currentUser { 
            get 
            {
                if (_user == null)
                {
                    return new User();
                }
                
                return _user;
            }
            set => _user = value; 
        }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<Lesson1Context>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Lesson1Context") ?? throw new InvalidOperationException("Connection string 'Lesson1Context' not found.")));

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

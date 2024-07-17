using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ResumeManager.DataCon;
using ResumeManager.Repository;

namespace ResumeManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            string sqlServerConnectionString = builder.Configuration.GetConnectionString("DbConn");
            builder.Services.AddDbContext<ResumeDBContext>(options =>
                    options.UseSqlServer(sqlServerConnectionString));
            //builder.Services.AddDbContext<ResumeDBContext>(opt=>opt.UseSqlServer(conf.GetCouchDBConnectionString("DbConn")));
            
            builder.Services.AddScoped<IApplicant, Applicant_Rep>();
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

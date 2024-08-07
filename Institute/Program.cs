using Institute.Models;
using Institute.Repository;
using Microsoft.EntityFrameworkCore;

namespace Institute
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
            });

            #region IOC Configuration
            builder.Services.AddScoped<InstituteContext, InstituteContext>();
            builder.Services.AddScoped<ICourseRepo, CourseRepository>();
            builder.Services.AddScoped<IDepartmentRepo, DepartmentRepository>();

            builder.Services.AddDbContext<InstituteContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("InstituteContext"));
            });

            #endregion
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseSession();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

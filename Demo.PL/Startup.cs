using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositries;
using Demo.DAL.Data;
using Demo.DAL.Models;
using Demo.PL.Extensions;
using Demo.PL.Helpers;
using Demo.PL.Helpers.Profiles;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(); // Register  Built-in MVC  Services to The Container --> By3adii 3la el Services wa7d wa7d w by allow dependency Injection

            //services.AddSingleton<AppDbContext>();// Da 8alat
            //services.AddScoped<AppDbContext>();// Da 27san 7aga

            //Kda m4 h7tag 23ml Override l OnConfiguring 
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                
            }); //Bsta5dem de

            services.AddApplicationServices();//Extension Method

            services.AddAutoMapper(M=> M.AddProfile(new MappingProfiles()));
            services.AddAutoMapper(M=> M.AddProfile(new UserProfile()));
            services.AddAutoMapper(M=> M.AddProfile(new RoleProfile()));


            //services.AddScoped<UserManager<ApplicationModel>>();
            //services.AddScoped<SignInManager<ApplicationModel>>();
            //services.AddScoped<RoleManager<IdentityRole>>();

            services.AddIdentity<ApplicationModel, IdentityRole>(config =>
            {
                config.Password.RequiredUniqueChars = 2;
                config.Password.RequireDigit = true;
                config.Password.RequireNonAlphanumeric = true; 
                config.Password.RequiredLength = 5;
                config.Password.RequireUppercase = true;
                config.User.RequireUniqueEmail = true;// BYW2EF ACCOUNT
                config.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                config.Lockout.AllowedForNewUsers = true; //ENABLE AND DISABLE

            }
            ).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Account/SignIn";
            });
  

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

         
            app.UseAuthentication();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

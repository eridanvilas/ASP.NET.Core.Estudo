﻿using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Exercicio.Claims;

namespace WebApp.Exercicio
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var connectionString = @"Server=192.168.10.51;Database=API.DB.Estudo;User Id=ged;Password=159159;";
            var migrationAssembly = typeof(Startup)
                .GetTypeInfo().Assembly
                .GetName().Name;

            services.AddDbContext<MyUserDbContext>(
               opt => opt.UseSqlServer(connectionString, 
               sql => sql.MigrationsAssembly(migrationAssembly))
            );

            services.AddIdentity<MyUser,IdentityRole>(options => 
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;

                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.AllowedForNewUsers = true;
            })
                .AddEntityFrameworkStores<MyUserDbContext>()
                .AddDefaultTokenProviders()
                .AddPasswordValidator<PasswordValidator<MyUser>>();

            services.AddScoped<IUserStore<MyUser>,
                UserOnlyStore<MyUser, MyUserDbContext>>();

            services.AddScoped<IUserClaimsPrincipalFactory<MyUser>,
            MyUserClaimsPrincipalFactory>();

            services.Configure<DataProtectionTokenProviderOptions>(
                options => options.TokenLifespan = TimeSpan.FromHours(3)
                );

            services.ConfigureApplicationCookie(options => options.LoginPath = "/Home/Login");


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseAuthentication();

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
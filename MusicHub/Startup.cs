using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicHub.Data;
using MusicHub.Models;
using MusicHub.Services;
using Microsoft.AspNetCore.Identity;
using MusicHub.Classes;

namespace MusicHub
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //Adding registration and login rules.
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 10;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //Adding Authentications from providers, initialize data by secret configuration file.
            services.AddAuthentication()
            .AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = Configuration["Facebook:app_id"];
                facebookOptions.AppSecret = Configuration["Facebook:app_secret"];
            }).AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = Configuration["Google:client_id"];
                googleOptions.ClientSecret = Configuration["Google:client_secret"];

            }).AddMicrosoftAccount(microdoftOptions =>
            {
                microdoftOptions.ClientId = Configuration["Microsoft:app_id"];
                microdoftOptions.ClientSecret = Configuration["Microsoft:password"];
            });

            //configure email services
            services.AddSingleton<IEmailSender, EmailSender>();
            services.Configure<EmailSenderInfo>(Configuration);

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //Add users permissions roles.
            CreateRoles(serviceProvider).Wait();
        }

        /// <summary>
        /// Check if roles are exist, if not - assing them.
        /// </summary>
        /// <param name="serviceProvider">services</param>
        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            //initializing custom roles 
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleNames = new string[] { Consts.Admin, Consts.Member };

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    //create the roles and add them to the database
                    await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            //Creating an admin user
            var adminUser = new ApplicationUser
            {
                UserName = Configuration["Admin-Info:UserName"],
                Email = Configuration["Admin-Info:Email"],
                EmailConfirmed = true
            };

            string userPWD = Configuration["Admin-Info:Password"];
            //Checking if the user doesn't exist in order to add it
            var _user = await UserManager.FindByEmailAsync(adminUser.Email);
            if (_user == null)
            {
                //trying to add the user into the db
                var createAdminUser = await UserManager.CreateAsync(adminUser, userPWD);
                if (createAdminUser.Succeeded)
                {
                    //Add the Admin role to the user
                    await UserManager.AddToRoleAsync(adminUser, Consts.Admin);
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BethanysPieShopHRM.App.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BethanysPieShopHRM.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();            
            services.AddServerSideBlazor();

            services.AddHttpClient<IEmployeeDataService, EmployeeDataService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:44340/");              
            });
            services.AddHttpClient<ICountryDataService, CountryDataService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:44340/");
            });
            services.AddHttpClient<IJobCategoryDataService, JobCategoryDataService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:44340/");
            });

            //services.AddAuthentication("Identity.Application")
            //    .AddCookie();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme,
                  options =>
                  {
                      options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                      options.Authority = "https://localhost:44333";
                      options.ClientId = "bethanyspieshophr";
                      options.ClientSecret = "108B7B4F-BEFC-4DD2-82E1-7F025F0F75D0";
                      options.ResponseType = "code";
                      options.Scope.Add("openid");
                      options.Scope.Add("profile");
                      options.Scope.Add("email");
                      options.Scope.Add("bethanyspieshophrapi");
                      options.Scope.Add("offline_access");
                      //options.CallbackPath = ...
                      options.SaveTokens = true;
                      options.GetClaimsFromUserInfoEndpoint = true;
                      options.TokenValidationParameters.NameClaimType = "given_name";
                  });

            services.AddScoped<TokenProvider>();
            services.AddScoped<TokenManager>();

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
                app.UseExceptionHandler("/Error");
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
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}

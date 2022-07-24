using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Movies.Client.ApiServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using IdentityModel;
using Microsoft.Net.Http.Headers;
using Movies.Client.HttpHandler;
using IdentityModel.Client;

namespace Movies.Client
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
            services.AddControllersWithViews();
            services.AddScoped<IMovieApiService, MovieApiService>();

            services.AddTransient<AuthenticationDelegatingHandler>();

            services.AddHttpClient("MovieAPIClient", client =>
           {
               client.BaseAddress = new Uri("https://localhost:5001"); // API GATEWAY URL
               client.DefaultRequestHeaders.Clear();

               client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
           }).AddHttpMessageHandler<AuthenticationDelegatingHandler>();

            // 2 create an HTTPClient used for accessing the IS4
            services.AddHttpClient("IDPClient", client =>
           {
               client.BaseAddress = new Uri("https://localhost:5005"); // IS4
               client.DefaultRequestHeaders.Clear();
               client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
           });


            services.AddHttpContextAccessor();

            // Removed this because I am using the Hybrid flow now and using HttpContexe Accessor to retrieve the existing token
            //services.AddSingleton(new ClientCredentialsTokenRequest
            //{
            //    Address = "https://localhost:5005/connect/token",
            //    ClientId = "movieClient",
            //    ClientSecret = "secret",
            //    Scope = "movieAPI"
            //});

 

            services.AddAuthentication(options =>
               {
                   options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                   options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
               })
               .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme , options => {
                   options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Movies/AccessDenied");
               })
               .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
               {
                   options.Authority = "https://localhost:5005";

                   options.ClientId = "movies_mvc_client";
                   options.ClientSecret = "secret";
                  // options.ResponseType = "code";
                    options.ResponseType = "code id_token";   // added for Hybrid Flow

                   options.Scope.Add("openid");   // not mandatory its added by defailt
                   options.Scope.Add("profile");  // not mandatory
                   options.Scope.Add("movieAPI");       // added for Hybrid Flow
                   options.Scope.Add("email");       // added for Hybrid Flow
                   options.Scope.Add("movieAPI");       // added for Hybrid Flow
                   options.Scope.Add("roles");       // added for roles based authorization
                   options.ClaimActions.MapUniqueJsonKey("role", "role");       // added for roles based authorization
                   
                   options.SaveTokens = true;
                   options.GetClaimsFromUserInfoEndpoint = true;

                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       NameClaimType = JwtClaimTypes.GivenName,
                       RoleClaimType = JwtClaimTypes.Role   // added for roles based authorization, If name and Role are not presesnt Identtiy server will give unauthorized message
                   };
                   
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

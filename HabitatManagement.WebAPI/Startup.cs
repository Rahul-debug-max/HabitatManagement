using HabitatManagement.BusinessEntities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace HabitatManagement.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            DBConfiguration.SetConfig(Configuration);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();           
            services.AddSingleton<IConfiguration>(Configuration);       // add Configuration to our services collection
            services.AddTransient<IDBConfiguration, DBConfiguration>(); // register our IDBConfiguration class (from class library)
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };

                options.Events = new JwtBearerEvents
                {
                    //OnAuthenticationFailed = (context) =>
                    //{
                    //    Console.WriteLine(context.Exception);
                    //    return Task.CompletedTask;
                    //},
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        else if (context.Exception.GetType() == typeof(SecurityTokenValidationException))
                        {
                            context.Response.Headers.Add("Token-Validation", "false");
                        }
                        else if (context.Exception.GetType() == typeof(ArgumentException))
                        {
                            context.Response.Headers.Add("Token-Key-Invalid", "true");
                        }
                        return Task.CompletedTask;
                    },
                    OnMessageReceived = (context) =>
                    {
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = (context) =>
                    {
                        return Task.CompletedTask;
                    },
                    //OnChallenge = async context =>
                    //{
                    //    // Call this to skip the default logic and avoid using the default response
                    //    context.HandleResponse();          
                    //    // Write to the response in any way you wish
                    //    context.Response.StatusCode = 401;
                    //    context.Response.Headers.Append("Token-Invalid", "true");
                    //    await context.Response.WriteAsync("You are not authorized.");
                    //}
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
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MVC.Models;
using MVC.Components.Helpers;
using AspNetCoreRateLimit;

namespace MVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }
        private IWebHostEnvironment Environment{ get; set; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Request limiter: https://github.com/stefanprodan/AspNetCoreRateLimit
            var isProduction = false;
            if (Environment.EnvironmentName != "Develpoment") {isProduction = true;};

            if (isProduction == true) {
                services.AddMemoryCache();
                services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));
                services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
                services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            }

            services.AddLogging(loggingBuilder => {
        	    var loggingSection = Configuration.GetSection("Logging");
                loggingBuilder.AddFile(loggingSection);
            });
            services.AddSingleton<IErrorMessage, ErrorMessage>();
            services.AddMvc();
            // Get and log default offset
            TimeOffsetModel.offset = Configuration.GetValue<int>("Time:offset");
            Console.WriteLine("Startup.cs: Offset: " + TimeOffsetModel.offset);

            // Request limiter: https://github.com/stefanprodan/AspNetCoreRateLimit
            if (isProduction == true) {
                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseIpRateLimiting();
            app.UseExceptionHandler("/Error");
            app.UseHsts();
            app.Use(async (context, next) =>
            {
                var ip = context.Connection.RemoteIpAddress.ToString();
                context.Items.Add("ip", ip);
                await next();
                if (context.Response.StatusCode == 404)
                {
                    context.Request.Path = "/Error";
                    await next();
                }
            });
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
         endpoints.MapControllers();
    });
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NCELAP.WebAPI.Data;
using NCELAP.WebAPI.Services;
using NCELAP.WebAPI.Services.Application;
using NCELAP.WebAPI.Services.Support;
using NCELAP.WebAPI.Util;

namespace NCELAP.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddCors(options =>
            {
                options.AddPolicy("CorsApiPolicy",
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:44374", "https://ncelap-demo.azurewebsites.net")
                            .WithHeaders(new[] { "authorization", "content-type", "accept" })
                            .WithMethods(new[] { "GET", "POST", "PUT", "DELETE", "OPTIONS" });
                    });
            });
            services.AddTransient<ISupportTicket, SupportTicketsService>();
            services.AddControllers();
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "NCELAP Web API",
                    Description = "This is the official documentation website for the NCELAP Web API",
                    Contact = new OpenApiContact
                    {
                        Name = "Wragby Business Solutions",
                        Email = "info@wragbysolutions.com",
                        Url = new Uri("https://wragbysolutions.com/"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Department of Petroluem Resources",
                        Url = new Uri("https://www.dpr.gov.ng/"),
                    }
                });
            });
            //services.AddHttpClient("remita", config =>
            //{
            //    config.DefaultRequestHeaders.Clear();
            //    config.BaseAddress = new Uri(Configuration.GetSection("Remita").GetSection("baseUrl").Value);
            //}).ConfigurePrimaryHttpMessageHandler(() =>
            //{
            //    //use Fiddler 
            //    var httpClientHandler = new HttpClientHandler()
            //    {
            //        AllowAutoRedirect = false
            //    };
            //    return new DisableActivityHandler(httpClientHandler);
            //});
            services.AddSwaggerGenNewtonsoftSupport();
            services.AddSingleton(Configuration.GetSection("Remita").Get<RemitaAppSetting>());
            services.AddTransient<IRemitaService, RemitaService>();
            services.AddTransient<IPaymentService, PaymentService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "NCELAP Web API");
                c.RoutePrefix = string.Empty;
            });

            app.UseCors("CorsApiPolicy");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //Disable Correlation for Remita

        }

    }
}
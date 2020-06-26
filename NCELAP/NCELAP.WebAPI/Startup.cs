using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;


namespace NCELAP.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            //var appBasePath = System.IO.Directory.GetCurrentDirectory();
            //NLog.GlobalDiagnosticsContext.Set("appbasepath", appBasePath);
            //var logger = LogManager.LoadConfiguration("nlog.config").GetCurrentClassLogger();
            //var logger = LogManager.LoadConfiguration("nlog.config").GetCurrentClassLogger();

            //LogManager.LoadConfiguration(System.String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo {
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
            services.AddSwaggerGenNewtonsoftSupport();
            services.AddApiVersioning();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}

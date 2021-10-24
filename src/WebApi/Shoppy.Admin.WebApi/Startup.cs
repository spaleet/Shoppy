using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using _0_Framework.Application.Behaviours;
using _0_Framework.Presentation.Extensions.Startup;
using FluentValidation;
using MediatR;
using Newtonsoft.Json;
using SM.Application;
using SM.Infrastructure.Configuration;
using SM.Infrastructure.Shared.Mappings;

namespace Shoppy.Admin.WebApi
{
    public class Startup
    {
        #region Ctor

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        #endregion

        #region ConfigureServices

        public void ConfigureServices(IServiceCollection services)
        {
            #region Configuring Modules

            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            ShopManagementBootstrapper.Configure(services, connectionString);

            #endregion

            services.AddMediatorAndFluentValidationExtension(new List<Type>
            {
                typeof(Startup),
                typeof(ISMAssemblyMarker)
            });

            #region AutoMapper

            services.AddAutoMapper((serviceProvider, autoMapper) =>
            {
                autoMapper.AddProfile(new ShopManagementMappingProfile());
            }, typeof(Startup).Assembly);

            #endregion

            #region Swagger

            var xmlFile = Assembly.GetExecutingAssembly().GetName().Name + ".xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            services.AddSwaggerExtension("Shoppy.Admin.WebApi", xmlPath);

            #endregion

            #region MVC Configuration

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.NullValueHandling = NullValueHandling.Include;
                options.SerializerSettings.MaxDepth = int.MaxValue;
                options.SerializerSettings.Formatting = Formatting.Indented;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            #endregion
        }

        #endregion

        #region Configure

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseSwaggerExtension("Shoppy.Admin.WebApi");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        #endregion
    }
}

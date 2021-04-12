using System;
using System.Collections.Generic;
using System.Linq;
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

using ImmunizationApi.Repository;
using ImmunizationApi.Repository.Example;
using ImmunizationApi.Services;
using ImmunizationApi.Services.Example;

namespace ImmunizationApi
{
    public class Startup
    {
        public readonly string AllowSpecificOrigins = "AllowAll";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ImmunizationDbContext>(opt => opt.UseInMemoryDatabase(databaseName: "Immunizations"));

            services.AddMvcCore();

            services.AddScoped<IImmunizationRepository, ImmunizationRepository>();
            services.AddScoped<IImmunizationService, ImmunizationService>();

            services.AddHttpContextAccessor();
            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy(
                        AllowSpecificOrigins,
                        builder =>
                        {
                            builder
                                .WithOrigins("http://localhost:4200")
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowCredentials();
                        });
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ImmunizationApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ImmunizationApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(AllowSpecificOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Creating a new scope as the default dbContext is scoped.
            using var serviceScope = app.ApplicationServices.CreateScope();
            var context = serviceScope.ServiceProvider.GetService<ImmunizationDbContext>();
            SampleDataInitializer.Seed(context);
        }
    }
}

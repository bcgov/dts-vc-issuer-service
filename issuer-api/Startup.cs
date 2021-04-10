using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

using Issuer.Models;
using Issuer.Infrastructure;
using System.Reflection;
using System.IO;
using Microsoft.OpenApi.Models;
using Serilog;
using Microsoft.Extensions.Hosting;
using Issuer.Services;
using Issuer.HttpClients;

namespace Issuer
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public readonly string AllowSpecificOrigins = "AllowAll";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ILookupService, LookupService>();
            services.AddScoped<IVerifiableCredentialService, VerifiableCredentialService>();
            services.AddScoped<IPatientService, PatientService>();

            ConfigureClients(services);

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new EmptyStringToNullJsonConverter());
                });

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

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "VC Issuer API", Version = "v1" });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.AddSwaggerGenNewtonsoftSupport();

            services.AddHttpContextAccessor();

            services.AddAuthorization();

            ConfigureDatabase(services);

            // TODO: uncomment these lines to enable JWT token verification
            // var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_SIGNING_KEY"));
            // services.AddAuthentication(x =>
            // {
            //     x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //     x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            // })
            // .AddJwtBearer(x =>
            // {
            //     x.RequireHttpsMetadata = false;
            //     x.TokenValidationParameters = new TokenValidationParameters
            //     {
            //         ValidateIssuerSigningKey = true,
            //         IssuerSigningKey = new SymmetricSecurityKey(key),
            //         ValidateIssuer = false,
            //         ValidateAudience = false
            //     };
            // });
        }

        protected void ConfigureClients(IServiceCollection services)
        {
            // Clients
            services.AddHttpClient<IVerifiableCredentialClient, VerifiableCredentialClient>(client =>
            {
                client.BaseAddress = new Uri(IssuerEnvironment.VerifiableCredentialApi.Url.EnsureTrailingSlash());
                client.DefaultRequestHeaders.Add("x-api-key", IssuerEnvironment.VerifiableCredentialApi.Key);
            });

            services.AddHttpClient<IImmunizationClient, ImmunizationClient>(client =>
            {
                client.BaseAddress = new Uri(IssuerEnvironment.ImmunizationApi.Url.EnsureTrailingSlash());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ISSUER API V1");
            });

            ConfigureLogging(app);

            // Matches request to an endpoint
            app.UseRouting();
            app.UseCors(AllowSpecificOrigins);

            app.UseAuthentication();
            // app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        protected virtual void ConfigureDatabase(IServiceCollection services)
        {
            string connectionString;
            if (IssuerEnvironment.IsLocal)
            {
                connectionString = Configuration.GetConnectionString("IssuerDatabase");
            }
            else
            {
                connectionString = IssuerEnvironment.Postgres.ConnectionString;
            }

            services.AddDbContext<ApiDbContext>(options =>
                options.UseNpgsql(connectionString)
            );
        }

        protected virtual void ConfigureLogging(IApplicationBuilder app)
        {
            // Only logs components that appear after it in the pipeline, which
            // can be used to exclude noisy handlers from logging
            app.UseSerilogRequestLogging();
        }
    }
}

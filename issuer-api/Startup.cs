using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

using Prime.Models;
using Prime.Infrastructure;
using System.Reflection;
using System.IO;
using Microsoft.OpenApi.Models;
using Serilog;
using Microsoft.Extensions.Hosting;
using Prime.Services;
using Prime.HttpClients;

namespace Prime
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
                options.AddPolicy(AllowSpecificOrigins,
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
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
                client.BaseAddress = new Uri(PrimeEnvironment.VerifiableCredentialApi.Url.EnsureTrailingSlash());
                client.DefaultRequestHeaders.Add("x-api-key", PrimeEnvironment.VerifiableCredentialApi.Key);
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "PRIME API V1");
            });

            ConfigureLogging(app);

            // Matches request to an endpoint
            app.UseRouting();
            app.UseCors(AllowSpecificOrigins);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        protected virtual void ConfigureDatabase(IServiceCollection services)
        {
            string connectionString;
            if (PrimeEnvironment.IsLocal)
            {
                connectionString = Configuration.GetConnectionString("PrimeDatabase");
            }
            else
            {
                connectionString = PrimeEnvironment.Postgres.ConnectionString;
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

using Arch.EntityFrameworkCore.UnitOfWork;
using TesteApi.DataAccess;
using TesteApi.Models;
using TesteApi.Services.Helpers;
using TesteApi.WebAPI.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using Services.Planets;
using Services.Interfaces;

namespace TesteApi.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
            _isDevelopment = environment.IsDevelopment();
        }

        public IConfiguration Configuration { get; }

        private bool _isDevelopment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddErrorHandlingMiddleware();
            services.AddHttpClient();
            services.AddControllersWithViews();

            #region AppSettings

            var appSettings = new AppSettings();
            new ConfigureFromConfigurationOptions<AppSettings>(
                Configuration.GetSection("AppSettings"))
                    .Configure(appSettings);

            services.AddSingleton(appSettings);
            services.AddControllers()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.Configure<PlanetConfiguration>(Configuration.GetSection("PlanetConfiguration"));

            #endregion

            #region Swagger

            // Configurando o serviço de documentação do Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo()
                    {
                        Title = "Teste API",
                        Version = "v1",
                        Description = "API Teste",
                        Contact = new OpenApiContact
                        {
                            Name = "Teste",
                            Url = new Uri("https://swapi.dev/documentation")
                        }
                    });

                // Swagger 2.+ support
                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            #endregion

            // services.AddDbContext<Context>(options => options.UseSqlServer(Environment.GetEnvironmentVariable("CONNECTION_STRING")), ServiceLifetime.Transient)
            //     .AddUnitOfWork<Context>();

            AddScopedServices(services);
        }

        private static void AddScopedServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<HttpHelper>();
            services.AddMemoryCache();
            services.AddScoped<IPlanetService, PlanetService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AppSettings appSettings)
        {
            if (env.EnvironmentName == Environments.Development)
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHttpsRedirection();
                app.UseHsts();
            }

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseErrorHandlingMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

            #region Swagger

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "help/{documentName}/docs.json";
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"{appSettings.PathTesteApp}/help/v1/docs.json", "Teste Api");
                c.RoutePrefix = "help";
            });

            #endregion
        }
    }
}

using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Byndyusoft.AspNetCore.Instrumentation.Tracing.Serialization.Json;
using Byndyusoft.ValidEnumConverter;
using Infrastructure.Tracing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace ExchangeRates.Api;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.CustomSchemaIds(type => type.ToString().Replace("+", "."));
            options.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = "FrontOffice API" });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            options.IncludeXmlComments(xmlPath);

            xmlFile = $"{Assembly.GetAssembly(typeof(Startup))!.GetName().Name}.xml";
            xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            options.IncludeXmlComments(xmlPath);

            options.AddSecurityDefinition("token",
                                          new OpenApiSecurityScheme
                                          {
                                              Type = SecuritySchemeType.ApiKey,
                                              In = ParameterLocation.Header,
                                              Name = "Authorization",
                                              Description = "Токен авторизации"
                                          });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
                                           {
                                                       {
                                                               new OpenApiSecurityScheme
                                                               {
                                                                       Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "token" }
                                                               },
                                                               Array.Empty<string>()
                                                       }
                                           });
        });


        services.AddLocalization(options => options.ResourcesPath = "Resources");

        services.AddHttpClient();

        services.AddDateOnlyTimeOnlyStringConverters();

        services.AddControllers(mvc =>
            {
                mvc.ModelMetadataDetailsProviders.Add(new SystemTextJsonValidationMetadataProvider());
            })
            .AddJsonOptions(json =>
            {
                json.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                json.JsonSerializerOptions.Converters.AddValidEnumConverter();
            })
            .AddTracing(options =>
            {
                options.ValueMaxStringLength = 5000;
                options.Formatter = new SystemTextJsonFormatter
                {
                    Options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
                    {
                        Converters =
                        {
                            new JsonStringEnumConverter()
                        }
                    }
                };
            });

        services.AddMemoryCache();

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();


        services.AddCors(options =>
        {
            options.AddPolicy("AnyOrigin", builder =>
            {
                builder
                        .SetIsOriginAllowed(_ => true)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .WithExposedHeaders("Content-Disposition");
            });
        });

        services.AddApiTracing(
                Configuration.GetServiceName(),
                Configuration.GetSection("Jaeger").Bind);

        services.AddHealthChecks();
       
        services
                .AddExchangeRatesDomain()
                .AddExchangeRatesDataAccess(Configuration.GetRequiredConnectionString("ExchangeRates"));

    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseForwardedHeaders();

        if (env.IsDevelopment()) app.UseDeveloperExceptionPage();


        if (env.IsDevelopment() || env.IsStaging())
            app.UseSwagger()
               .UseSwaggerUI(options =>
               {
                   options.SwaggerEndpoint("/swagger/v1/swagger.json", "FrontOffice API v1");

                   options.DisplayRequestDuration();
                   options.DefaultModelRendering(ModelRendering.Model);
                   options.DefaultModelExpandDepth(3);
               });

        app.UsePathBase(new PathString("/api"));
        app.UseRouting();

        app.UseCors("AnyOrigin");

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapPrometheusScrapingEndpoint();
        });

        app.UseReadinessProbe().UseLivenessProbe();
    }
}

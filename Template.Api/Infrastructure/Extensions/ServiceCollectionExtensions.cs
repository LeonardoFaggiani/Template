using System.Reflection;
using System.Text;

using MediatR;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

using NetDevPack.Mediator;

using Template.Api.CrossCutting.Bus;
using Template.Api.Domian.Events;
using Template.Api.Infrastructure.Data;
using Template.Api.Infrastructure.Exceptions.Builder;
using Template.Api.Infrastructure.Filters;
using Template.Api.Infrastructure.HealthChecks;
using Template.Api.Infrastructure.Repositories;
using Template.Api.Infrastructure.Repositories.Base;

namespace Template.Api.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private const string SwaggerDocVersion = "v1";
        private static string AssemblyName => Assembly.GetEntryAssembly().GetName().Name;
        private static string ApiName => Assembly.GetExecutingAssembly().GetName().Name;

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(opt =>
            {
                opt.DescribeAllParametersInCamelCase();

                opt.SwaggerDoc(SwaggerDocVersion, new OpenApiInfo()
                {
                    Title = AssemblyName,
                    Version = SwaggerDocVersion
                });

                string apiXmlFile = $"{AssemblyName}.xml";
                string apiXmlPath = Path.Combine(AppContext.BaseDirectory, apiXmlFile);
                opt.IncludeXmlComments(apiXmlPath);

                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    In = ParameterLocation.Header,
                    Name = HeaderNames.Authorization,
                    Scheme = "Bearer",
                    Type = SecuritySchemeType.ApiKey
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        Array.Empty<string>()
                    }
                });
            });

            return services;
        }

        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            app.UseSwagger();

            if (!environment.IsProduction())
            {
                app.UseSwaggerUI(opt =>
                {
                    string url = $"./swagger/{SwaggerDocVersion}/swagger.json";

                    string name = $"{ApiName} - {SwaggerDocVersion}";

                    opt.SwaggerEndpoint(url, name);
                    opt.RoutePrefix = string.Empty;

                    opt.EnableDeepLinking();
                });
            }

            return app;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<ISampleRepository, SampleRepository>();
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            return services;
        }
        
        public static IServiceCollection AddBuilders(this IServiceCollection services)
        {
            services.AddTransient<IExceptionMessageBuilder, ExceptionMessageBuilder>();

            return services;
        }

        public static IServiceCollection AddConnectionString(this IServiceCollection services)
        {
            //In memory database used for simplicity, change to a real db for production applications
            services.AddDbContext<TemplateContext>(options => { options.UseInMemoryDatabase("templateApiBD"); }, ServiceLifetime.Transient);
            //services.AddDbContext<TemplateContext>(options => { options.UseSqlServer("your-connection-string"); }, ServiceLifetime.Transient);

            return services;
        }

        public static IServiceCollection AddAuthorization(this IServiceCollection services, IConfiguration configuration)
        {            
            var secret = configuration.GetSection("JwtOptions:Secret").Value;

            services.AddMvcCore(opt =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                opt.Filters.Add(typeof(CustomExceptionFilter));

            })
            .AddApiExplorer()
            .AddDataAnnotations();            

            services.AddCors();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            return services;
        }

        public static IServiceCollection AddDomainEvents(this IServiceCollection services)
        {
            services.AddScoped<IMediatorHandler, InMemoryBus>();

            services.AddScoped<INotificationHandler<SampleHasBeenInserted>, SampleEventHandler>();

            return services;
        }

        public static IApplicationBuilder UseHealthChecks(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = HealthCheckResponseWriter.Write
            });
            return app;
        }
    }
}
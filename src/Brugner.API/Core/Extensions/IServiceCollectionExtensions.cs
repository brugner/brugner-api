using Brugner.API.Core.Contracts.Repositories;
using Brugner.API.Core.Contracts.Services;
using Brugner.API.Core.Options;
using Brugner.API.Core.Services;
using Brugner.API.Infrastructure.Repositories;
using Brugner.API.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Brugner.API.Core.Extensions
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Register the services needed by the application.
        /// </summary>
        /// <param name="services">Service collection.</param>
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IHashService, HashService>();
            services.AddScoped<IPostsService, PostsService>();
            services.AddScoped<IDbManagerService, DbManagerService>();

            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IPostsRepository, PostsRepository>();
        }

        /// <summary>
        /// Adds JSON Web Token authentication.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <param name="jwtOptions">Jwt Options.</param>
        public static void AddJwtAuthentication(this IServiceCollection services, JwtOptions jwtOptions)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters()
                   {
                       ValidateActor = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       ValidIssuer = jwtOptions.Issuer,
                       ValidAudience = jwtOptions.Audience,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret))
                   };
               });
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Brugner API", Version = "v1" });
                options.EnableAnnotations();
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
        }
    }
}


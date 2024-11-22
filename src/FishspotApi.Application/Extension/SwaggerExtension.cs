using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace FishSpotApi.Application.Extension;

public static class SwaggerExtension
{
    public static IServiceCollection AddSwaggerGenWithAuth(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Description = "JWT Authorization header",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT",
            });
            
            options.AddSecurityRequirement(
                new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Scheme = "oauth2",
                            In = ParameterLocation.Header,
                            Name = JwtBearerDefaults.AuthenticationScheme,
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            },
                        },
                        new List<string>()
                    }
                }
            );
        
            options.SwaggerDoc("v1", 
                new OpenApiInfo 
                {
                    Version = "v0.0.1",
                    Title = "FishSpot API",
                    Description = "The best fish spot API",
                    Contact = new OpenApiContact()
                    {
                        Email = "gabrielalves.dev@gmail.com",
                        Name = "Gabriel Alves de Moura",
                        Url = new Uri("https://github.com/gbramoura"),
                    }                       
                }
            );
        });

        return services;
    }
}
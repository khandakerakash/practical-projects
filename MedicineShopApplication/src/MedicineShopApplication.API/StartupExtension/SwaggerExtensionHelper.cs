using Microsoft.OpenApi.Models;

namespace MedicineShopApplication.API.StartupExtension
{
    public static class SwaggerExtensionHelper
    {
        public static IServiceCollection AddSwaggerExtensionHelper(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                // Add the security definition for the Bearer token
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\n\nExample: \"Bearer 12345abcdef\""
                });

                // Add a global security requirement for the Bearer token
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

            return services;
        }
    }
}

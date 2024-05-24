using Microsoft.OpenApi.Models;

namespace MyApi.Services
{
    public static class swaggerService
    {
        public static IServiceCollection AddswaggerDoc(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "AppDemo",
                    Version = "v1"
                });

                var securityScema = new OpenApiSecurityScheme
                {
                    Description = "jwt description",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "bearer"
                    }

                };
                c.AddSecurityDefinition("bearer", securityScema);
                var securityReq = new OpenApiSecurityRequirement
                {
                    {securityScema,new[]{"bearer"} }
                };
                c.AddSecurityRequirement(securityReq);
            });
            return services; 
        }
    }
}

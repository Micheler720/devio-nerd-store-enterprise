using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NetDevPack.Security.Jwt.AspNetCore;
using NSE.Identidade.API.AutoMapper;
using NSE.Identidade.API.Services;
using NSE.WebAPI.Core.Identidade;
using NSE.WebAPI.Core.Usuario;

namespace NSE.Identidade.API.Configuration
{
    public static class ApiConfig
    {
        public static IServiceCollection AddApiConfig(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddAutoMapper(typeof(MapperView).Assembly);

            services.AddScoped<IAspNetUser, AspNetUser>();
            services.AddScoped<AuthenticationService>();
            return services;
        }

        public static IApplicationBuilder UseApiConfiguration(this IApplicationBuilder app, bool isDevelopment)
        {
            if (isDevelopment)
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthConfiguration();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseJwksDiscovery();
            return app;
        }
    }
}

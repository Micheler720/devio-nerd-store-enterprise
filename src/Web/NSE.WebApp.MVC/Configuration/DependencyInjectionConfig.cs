using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using NSE.WebAPI.Core.Extensions;
using NSE.WebAPI.Core.Usuario;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Services;
using NSE.WebApp.MVC.Services.Handles;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using System;
using System.Net.Http;

namespace NSE.WebApp.MVC.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IValidationAttributeAdapterProvider, CpfValidationAttributeAdapterProvider>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            #region HttpServices

                //Pega o WEb Token 
                services.AddTransient<HttpClientAuthorizantionDelagatingHandle>();

                services.AddHttpClient<ICatalogoService, CatalogoService>()
                    .AddHttpMessageHandler<HttpClientAuthorizantionDelagatingHandle>()
                    //.AddTransientHttpErrorPolicy(
                    //    p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(600))
                    //)
                    .AddPolicyHandler(PollyExtensions.EsperarTentar())
                    .AddTransientHttpErrorPolicy(
                        p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

                services.AddHttpClient<IComprasBffService, ComprasBffService>()
                    .AddHttpMessageHandler<HttpClientAuthorizantionDelagatingHandle>()
                    .AddPolicyHandler(PollyExtensions.EsperarTentar())
                    .AddTransientHttpErrorPolicy(
                        p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

                services.AddHttpClient<IClienteService, ClienteService>()
                    .AddHttpMessageHandler<HttpClientAuthorizantionDelagatingHandle>()
                    .AddPolicyHandler(PollyExtensions.EsperarTentar())
                    .AddTransientHttpErrorPolicy(
                        p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

                services.AddHttpClient<IAutenticacaoService, AutenticacaoService>()
                        .AddPolicyHandler(PollyExtensions.EsperarTentar())
                        .AddTransientHttpErrorPolicy(
                            p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

                
            #endregion


            return services;
        }
    }


}

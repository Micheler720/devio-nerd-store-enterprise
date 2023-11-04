using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSE.WebApp.MVCTeste.Data;

[assembly: HostingStartup(typeof(NSE.WebApp.MVCTeste.Areas.Identity.IdentityHostingStartup))]
namespace NSE.WebApp.MVCTeste.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<NSEWebAppMVCTesteContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("NSEWebAppMVCTesteContextConnection")));

                services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<NSEWebAppMVCTesteContext>();
            });
        }
    }
}
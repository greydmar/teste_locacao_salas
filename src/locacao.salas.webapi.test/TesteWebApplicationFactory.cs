using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using mtgroup.db;
using Xunit.Sdk;

namespace mtgroup.locacao
{
    public class TesteWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "debug");

            var builder = Host.CreateDefaultBuilder();

            //var builder = new HostBuilder();
            //builder
            //    .UseContentRoot(AppDomain.CurrentDomain.BaseDirectory)
            //    .ConfigureHostConfiguration(config =>
            //    {
            //        config.AddEnvironmentVariables(prefix: "DOTNET_");

            //    })
            //    .ConfigureAppConfiguration((context, config) =>
            //    {
            //        config.AddEnvironmentVariables();
            //    })
            //    .ConfigureLogging((context, logging) =>
            //    {
            //        logging.AddConfiguration(context.Configuration.GetSection("Logging"));
            //        logging.AddConsole();
            //        logging.AddDebug();
            //    })
            //    .UseDefaultServiceProvider((context, options) =>
            //    {
            //        bool isDevelopment = context.HostingEnvironment.IsDevelopment();
            //        options.ValidateScopes = isDevelopment;
            //        options.ValidateOnBuild = isDevelopment;
            //    });


            return builder
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<WebTesteStartup>();
                    webBuilder.UseContentRoot(AppDomain.CurrentDomain.BaseDirectory);
                    webBuilder
                        .ConfigureLogging(logging =>
                        {
                            logging.ClearProviders();
                            //logging.AddXunit(new TestOutputHelper());
                        })
                        .ConfigureTestServices((services) =>
                        {
                            services
                                .AddControllers()
                                .AddApplicationPart(typeof(Startup).Assembly);
                        });
                });
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            var createdHost = base.CreateHost(builder);

            createdHost
                .InicializarBdSqLiteTesteIntegracao();

            return createdHost;    
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddEnvironmentVariables();
            });
            
            base.ConfigureWebHost(builder);
        }
    }
}

using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using mtgroup.auth;
using mtgroup.db;

namespace mtgroup.locacao
{
    public sealed class WebTesteStartup
    {
        private IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly Startup _apiStartup;

        public WebTesteStartup(
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;

            _apiStartup = new Startup(_configuration);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            _apiStartup.ConfigureServices(services);


            var descriptor = services.SingleOrDefault
                (d => d.ServiceType == typeof(DbContextOptions<ContextoLocacaoSalas>));

            if (descriptor != null)
                services.Remove(descriptor);

            descriptor = services.SingleOrDefault
                (d => d.ServiceType == typeof(DbContextOptions<ContextoAutorizacao>));

            if (descriptor != null)
                services.Remove(descriptor);

            //EF-Context
            services.AddDbContext<ContextoLocacaoSalas>(ContextoEfCoreSqlite.SetupTesteIntegracaoOptions);
            services.AddDbContext<ContextoAutorizacao>(ContextoEfCoreSqlite.SetupTesteIntegracaoOptions);

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            _apiStartup.Configure(app, env, provider);
        }
    }
}
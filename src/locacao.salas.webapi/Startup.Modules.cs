using System.Linq;
using locacao.auth.core.Interfaces;
using locacao.auth.core.Servicos;
using locacao.clientebd;
using locacao.clientebd.Repositorios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using mtgroup.auth;
using mtgroup.auth.Repositorios;
using mtgroup.locacao.Interfaces;
using mtgroup.locacao.Interfaces.Repositorios;
using mtgroup.locacao.Interfaces.Servicos;
using mtgroup.locacao.Servicos;
using mtgroup.locacaosalas.Auxiliares;

namespace mtgroup.locacaosalas
{
    public partial class Startup
    {
        private static ILoggerFactory CreateFactory()
        {
            var factory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole()
                    .SetMinimumLevel(LogLevel.Debug);
            });
            return factory;
        }

        public static void SetupOptions(DbContextOptionsBuilder dbCtxBuilder)
        {
            dbCtxBuilder
                .UseSqlite("FileName=TestContextoLocacaoSalas.db")
                .EnableSensitiveDataLogging()
                .UseLoggerFactory(CreateFactory())
                ;
        }

        private void ConfigureModules(IServiceCollection services)
        {
            var descriptor = services.SingleOrDefault
                (d => d.ServiceType == typeof(DbContextOptions<ContextoLocacaoSalas>));

            if (descriptor != null)
                services.Remove(descriptor);

            descriptor = services.SingleOrDefault
                (d => d.ServiceType == typeof(DbContextOptions<ContextoAutorizacao>));

            if (descriptor != null)
                services.Remove(descriptor);

            //EF-Context
            services.AddDbContext<ContextoLocacaoSalas>(SetupOptions);
            services.AddDbContext<ContextoAutorizacao>(SetupOptions);

            ConfigureAuthorizationModule(services);
            ConfigureDomainModule(services);
        }
        
        private void ConfigureAuthorizationModule(IServiceCollection services)
        {
            services.AddScoped<IConsultaUsuarios, DbConsultaUsuarios>();
            services.AddScoped<IServicoAutorizacao, ServicoAutorizacaoUsuario>();
            services.AddScoped<IContextoExecucao, AuxiliarAcessoContextoExecucao>();
        }

        private void ConfigureDomainModule(IServiceCollection services)
        {
            services.AddSingleton<IServicoDataHora, ServicoDataHora>();

            services.AddScoped<IValidacaoRequisicao, ServicoValidacaoRequisicao>();
            services.AddScoped<IServicoAgendamento, ServicoAgendamento>();

            services.AddScoped<IRepositorioReservas, DbRegistroReservas>();
            services.AddScoped<IConsultaReservas, DbConsultaReservas>();
        }

    }
}
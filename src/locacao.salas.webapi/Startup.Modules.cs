using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using mtgroup.auth;
using mtgroup.auth.Interfaces;
using mtgroup.auth.Repositorios;
using mtgroup.auth.Servicos;
using mtgroup.db;
using mtgroup.locacao.Auxiliares;
using mtgroup.locacao.Interfaces;
using mtgroup.locacao.Interfaces.Repositorios;
using mtgroup.locacao.Interfaces.Servicos;
using mtgroup.locacao.Repositorios;
using mtgroup.locacao.Servicos;

namespace mtgroup.locacao
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
            ContextoEfCoreSqlite.SetupOptions(dbCtxBuilder);
            ////dbCtxBuilder.UseSqlite(
            ////        ContextoEfCoreSqlite.ConexaoArquivoFisico, 
            ////        builder =>
            ////        {
            ////            builder.MigrationsAssembly(typeof(ContextoEfCoreSqlite).Assembly.GetName().Name);
            ////        })
                
            ////    /*.EnableSensitiveDataLogging()
            ////    .UseLoggerFactory(CreateFactory())*/
            ////    ;
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
            services.AddScoped<IServicoAutenticacao, ServicoAutenticacaoUsuario>();
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
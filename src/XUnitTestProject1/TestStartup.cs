using locacao.clientebd;
using locacao.tests.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using mtgroup.locacao.Interfaces.Repositorios;
using mtgroup.locacao.Interfaces.Servicos;
using mtgroup.locacao.Servicos;

namespace locacao.tests
{
    public class TestStartup
    {
        public void ConfigureService(IServiceCollection services)
        {
            services.AddSingleton<IServicoDataHora, ServicoDataHora>();
            services.AddScoped<IConsultaReservas>(ConfiguracaoRepositorios.ConsultaReservas);
            services.AddScoped<IValidacaoRequisicao, ServicoValidacaoRequisicao>();
            
            //services.AddDbContext<ContextoLocacaoSalas>(s=> s.UseSqlite())
        }
    }
}

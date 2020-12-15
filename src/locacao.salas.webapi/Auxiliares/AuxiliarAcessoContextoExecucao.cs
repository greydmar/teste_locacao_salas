using Microsoft.AspNetCore.Http;
using mtgroup.locacao.DataModel;
using mtgroup.locacao.Interfaces;

namespace mtgroup.locacaosalas.Auxiliares
{
    public class AuxiliarAcessoContextoExecucao : IContextoExecucao
    {
        private readonly IHttpContextAccessor _httpCtx;

        public AuxiliarAcessoContextoExecucao(IHttpContextAccessor httpCtx)
        {
            _httpCtx = httpCtx;
        }

        public Solicitante Solicitante
        {
            get
            {
                return this._httpCtx.GetUser();
            }
        }
    }
}
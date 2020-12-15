using System.Threading.Tasks;
using FluentResults;
using mtgroup.locacao.DataModel;

namespace mtgroup.locacao.Interfaces.Servicos
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Desacoplamento simples entre as validações que são intrínsecas às regras fornecidas e o implementador específico de validação.
    /// Por economia de processamento, esta validação pode ser invocada em camadas de controle antes dos serviços de domínio
    /// </remarks>
    public interface IValidacaoRequisicao
    {
        Task<Result<RequisicaoSalaReuniao>> RequisicaoValida(RequisicaoSalaReuniao requisicao);
        bool EhPossivelSugerir(Result<RequisicaoSalaReuniao> result, RequisicaoSalaReuniao requisicao);
    }

}
using System;
using System.Linq;
using System.Threading.Tasks;
using Ag3.Util.Suporte;
using FluentResults;
using mtgroup.locacao.DataModel;
using mtgroup.locacao.Interfaces.Repositorios;
using mtgroup.locacao.Interfaces.Servicos;

namespace mtgroup.locacao.Servicos
{
    public class ServicoValidacaoRequisicao: IValidacaoRequisicao
    {
        private readonly IServicoDataHora _svcDataHora;
        private readonly IConsultaReservas _repConsultaReservas;

        public ServicoValidacaoRequisicao(
            IServicoDataHora svcDataHora,
            IConsultaReservas repConsultaReservas)
        {
            _svcDataHora = svcDataHora;
            _repConsultaReservas = repConsultaReservas;
        }

        Result ValidarAntecedencia(
            ContextoValidacao ctx, 
            RequisicaoSalaReuniao requisicao)
        {
            var dataHoje = ctx.ServicoData.DataHoje;
            var solicitadoPara = requisicao.Periodo.Inicio;

            if (solicitadoPara <= dataHoje)
                return ResultHelper.Fail("data_minima", "Data de Reserva informada deve ter no mínimo um dia de antecedência");

            if ((solicitadoPara - dataHoje) < TimeSpan.FromDays(1))
                return ResultHelper.Fail("data_minima","Data de Reserva informada deve ter no mínimo um dia de antecedência");

            if (solicitadoPara > (dataHoje + TimeSpan.FromDays(40)))
                return ResultHelper.Fail("data_maxima", "Data de Reserva deve ter no mínimo um dia de antecedência");

            return Result.Ok();
        }

        Result ValidarPeriodoLocacao(
            ContextoValidacao ctx,
            RequisicaoSalaReuniao requisicao)
        {
            var periodoLocacao = requisicao.Periodo;
            var min = TimeSpan.FromHours(1);
            var max = TimeSpan.FromHours(8);

            if (periodoLocacao < min)
                return ResultHelper.Fail("periodo_minimo", $"A locação de sala deve ser de ao menos {min:g}");

            if (periodoLocacao > max)
                ResultHelper.Fail("periodo_maximo", $"A locação de sala deve ser de no máximo {max:g}");

            if (!periodoLocacao.FinalNoMesmoDia())
                return ResultHelper.Fail("transbordo_data", $"O início e o final da locação devem estar no mesmo dia!");
            
            return Result.Ok();
        }

        async Task<Result> ValidarPerfilSalaRequisitada(
            ContextoValidacao ctx,
            RequisicaoSalaReuniao requisicao)
        {
            if (!await ctx.ConsultaReservas.ExistePerfilSala(requisicao))
                return ResultHelper.Fail("perfil_inexistente",
                    $"Não dispomos de salas com este perfil \"{requisicao.DescricaoPerfil()}\"");

            return Result.Ok();
        }

        Result ValidarDiaUtil(ContextoValidacao ctx, RequisicaoSalaReuniao requisicao)
        {
            DateTime inicioReserva = requisicao.Periodo;

            if (!ctx.ServicoData.EhDiaUtil(inicioReserva))
                return Result.Fail($"A data solicitada \"{inicioReserva:d}\" não é um dia útil");
                ////return Result.Fail(ValueTask.FromResult((false, "nao_eh_dia_util",
                ////    $"A data solicitada \"{inicioReserva:d}\" não é um dia útil"));

                return Result.Ok();
            //return ValueTask.FromResult((true, string.Empty, string.Empty));
        }

        async Task<Result> VerificarDisponibilidade(ContextoValidacao ctx, RequisicaoSalaReuniao requisicao)
        {
            if ((await ctx.ConsultaReservas.ListarSalasDisponiveis(requisicao.Periodo)).Any())
                return Result.Fail($"Nenhuma sala disponível no período \"{requisicao.Periodo}\" atende aos itens requisitados");

            return Result.Ok();

            //return ResultadoOperacao<RequisicaoSalaReuniao>
            //    .EhAceitavel("concorrencia_reservas", requisicao,
            //        (req) =>
            //        {
            //            var periodoLocacao = requisicao.Periodo;
            //            DateTime inicioReserva = requisicao.Periodo;

            //            if (!ctx.ServicoData.EhDiaUtil(inicioReserva))
            //                return (false, "nao_eh_dia_util",
            //                    $"A data solicitada \"{inicioReserva:d}\" não é um dia útil");

            //            return (true, string.Empty, string.Empty);
            //        });

        }

        public async Task<Result<RequisicaoSalaReuniao>> RequisicaoValida(RequisicaoSalaReuniao requisicao)
        {
            var ctx = new ContextoValidacao(this._repConsultaReservas, this._svcDataHora);

            return Result.Merge(
                ValidarAntecedencia(ctx, requisicao),
                ValidarPeriodoLocacao(ctx, requisicao),
                ValidarDiaUtil(ctx, requisicao),
                await ValidarPerfilSalaRequisitada(ctx, requisicao)
                /*await VerificarDisponibilidade(ctx, requisicao)*/
            );

            ////result.ToResult<RequisicaoSalaReuniao>();

            ////return ResultadoOperacao<RequisicaoSalaReuniao>
            ////    .EhAceitavelSeTodos("requisicao_valida", requisicao,
            ////        (req) => ValidarAntecedencia(ctx, req),
            ////        (req) => ValidarPeriodoLocacao(ctx, req),
            ////        (req) => ValidarDiaUtil(ctx, req)
            ////    );
        }

        public bool EhPossivelSugerir(Result<RequisicaoSalaReuniao> result, RequisicaoSalaReuniao requisicao)
        {
            return false;
        }
    }
}
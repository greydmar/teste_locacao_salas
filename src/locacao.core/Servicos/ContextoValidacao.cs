using System;
using mtgroup.locacao.Interfaces.Repositorios;
using mtgroup.locacao.Interfaces.Servicos;

namespace mtgroup.locacao.Servicos
{
    internal class ContextoValidacao
    {
        public ContextoValidacao(
            IConsultaReservas repConsultas
            , IServicoDataHora servicoDataHora)
        {
            ConsultaReservas = repConsultas;
            ServicoData = servicoDataHora;
        }

        public IServicoDataHora ServicoData { get; }

        public IConsultaReservas ConsultaReservas { get; }
    }
}
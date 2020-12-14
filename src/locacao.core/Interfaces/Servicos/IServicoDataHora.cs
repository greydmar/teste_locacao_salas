using System;

namespace mtgroup.locacao.Interfaces.Servicos
{
    public interface IServicoDataHora
    {
        DateTime DataHoje { get; }

        DateTime DataHoraHoje { get; }

        bool EhDiaUtil(DateTime dateTime);
    }
}
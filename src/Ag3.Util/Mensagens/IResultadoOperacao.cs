using System;
using System.Collections.Generic;

namespace Ag3.Util.Mensagens
{
    public interface IOcorrenciaOperacao
    {
        bool HaErros();

        IEnumerable<string> Mensagens { get; }

        Exception Exception { get; }
    }

    public interface IOcorrenciaComStatus<out TCod, out TStatus> : IOcorrenciaOperacao
        where TCod : IComparable<TCod>
        where TStatus : IComparable
    {
        public TCod Codigo { get; }

        public TStatus Status { get; }
    }

    public interface IResultadoOperacao
    {
        IOcorrenciaOperacao Ocorrencia { get; }
    }

    public interface IResultadoOperacao<out TEntrada>: IResultadoOperacao
        where TEntrada: class
    {
        TEntrada Entrada { get; }
    }
}
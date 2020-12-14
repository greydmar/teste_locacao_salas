using System;

namespace mtgroup.locacao.DataModel
{
    [Flags]
    public enum RecursoSalaReuniao
    {
        Nenhum = 0,
        AcessoInternet = 1,
        Televisor = 2,
        WebCam = 4,
        Computador = 8,
        VideoConferencia = AcessoInternet | Televisor | WebCam,
    }
}
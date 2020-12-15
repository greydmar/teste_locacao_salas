using FluentResults;

namespace locacao.auth.core.Servicos
{
    public class UsuarioNaoAutorizado : Error
    {
        public UsuarioNaoAutorizado() {}

        public UsuarioNaoAutorizado(string message) : base(message) {}

        public UsuarioNaoAutorizado(string message, Error causedBy) 
            : base(message, causedBy) { }
    }
}
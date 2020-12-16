using FluentResults;

namespace mtgroup.auth.Interfaces.Model
{
    public class UsuarioNaoAutorizado : Error
    {
        public UsuarioNaoAutorizado() {}

        public UsuarioNaoAutorizado(string message) : base(message) {}

        public UsuarioNaoAutorizado(string message, Error causedBy) 
            : base(message, causedBy) { }
    }
}
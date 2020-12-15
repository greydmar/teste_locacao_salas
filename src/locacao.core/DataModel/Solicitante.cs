using System;
using System.Security.Claims;
using System.Security.Principal;
using mtgroup.locacao.Interfaces;

namespace mtgroup.locacao.DataModel
{
    public class Solicitante: IEntidade, IPrincipal
    {
        private readonly ClaimsPrincipal _inner;

        public Solicitante(ClaimsPrincipal principal)
        {
            _inner = principal;

            var cId = principal.FindFirst(ClaimTypes.NameIdentifier);

            int id=0;
            if (cId == null || !int.TryParse(cId.Value, out id))
            {
                throw new ArgumentException("Identidade inválida. Id Solicitante não foi encontrado");
            }

            this.Id = id;
        }

        public Solicitante(string name, int id = 0)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            _inner = new GenericPrincipal(new GenericIdentity(name), Array.Empty<string>());
            this.Id = id;
        }

        public int Id { get; private set; }

        public bool IsInRole(string role)
        {
            return _inner?.IsInRole(role) ?? false;
        }

        public string Name => ((IPrincipal) this).Identity?.Name;

        IIdentity? IPrincipal.Identity => _inner.Identity;
    }
}
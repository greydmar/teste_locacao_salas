using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using mtgroup.locacao.Interfaces;

namespace mtgroup.locacao.DataModel
{
    public class Solicitante: IEntidade, IPrincipal
    {
        private ClaimsPrincipal _inner;

        private readonly string _name;
        
        public Solicitante(ClaimsPrincipal principal)
        {
            _inner = principal;

            var claimUserId = principal.Claims.FirstOrDefault(claim => claim.Type == ConfigAcessoInfoUsuario.ClaimNames.UserId);
            var clamUserLogin = principal.Claims.FirstOrDefault(claim => claim.Type == ConfigAcessoInfoUsuario.ClaimNames.UserLogin);

            if (claimUserId == null || !int.TryParse(claimUserId.Value, out var userId))
            {
                throw new ArgumentException("Identidade inválida. Id Solicitante não foi encontrado");
            }

            if (clamUserLogin == null || string.IsNullOrEmpty(clamUserLogin.Value))
            {
                throw new ArgumentException("Identidade inválida. Login do Solicitante não foi encontrado");
            }

            this.Id = userId;
            this._name = clamUserLogin.Value;
        }

        public Solicitante(string name, int id = 0)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            this.Id = id;
            this._name = name;
        }

        public int Id { get; private set; }

        public bool IsInRole(string role)
        {
            return _inner?.IsInRole(role) ?? false;
        }

        public string Name => _name;

        IIdentity? IPrincipal.Identity => ResolvePrincipal().Identity;

        private ClaimsPrincipal ResolvePrincipal()
        {
            if (_inner != null)
                return _inner;

            lock (this)
            {
                var claimUserId = new Claim(ConfigAcessoInfoUsuario.ClaimNames.UserId, Id.ToString("D", CultureInfo.InvariantCulture));
                var clamUserLogin = new Claim(ConfigAcessoInfoUsuario.ClaimNames.UserLogin, _name);

                _inner = new ClaimsPrincipal(new ClaimsIdentity(new[] {claimUserId, clamUserLogin}));
            }

            return _inner;
        }
        
        public static explicit operator ClaimsPrincipal(Solicitante self)
        {
            return self._inner ?? self.ResolvePrincipal();
        }
    }
}
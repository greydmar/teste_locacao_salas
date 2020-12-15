using System.Security.Principal;

namespace mtgroup.locacao.DataModel
{
    public class Solicitante: IPrincipal, IIdentity
    {
        public bool IsInRole(string role)
        {
            return true;
        }

        IIdentity? IPrincipal.Identity => this;

        public string? AuthenticationType { get; set; }
        
        public bool IsAuthenticated { get; set; }
        
        public string? Name { get; set; }
    }
}
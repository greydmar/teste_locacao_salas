using System.Security.Principal;

namespace mtgroup.locacao.DataModel
{
    public class Solicitante: IPrincipal
    {
        public bool IsInRole(string role)
        {
            throw new System.NotImplementedException();
        }

        public IIdentity? Identity { get; }
    }
}
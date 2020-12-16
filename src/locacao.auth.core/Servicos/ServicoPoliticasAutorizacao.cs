using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace mtgroup.auth.Servicos
{
    public interface IServicoPoliticasAutorizacao
    {
        (string, AuthorizationPolicy)[] ListarPoliticas();
    }

    public class ServicoPoliticasAutorizacao: IServicoPoliticasAutorizacao
    {
        public (string, AuthorizationPolicy)[] ListarPoliticas()
        {
            return new (string, AuthorizationPolicy)[]
            {
                
            }
        }
    }
}

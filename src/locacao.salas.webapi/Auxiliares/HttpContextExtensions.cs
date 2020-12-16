using System;
using Microsoft.AspNetCore.Http;
using mtgroup.auth.Interfaces;
using mtgroup.locacao.DataModel;

namespace mtgroup.locacao.Auxiliares
{
    internal static class HttpContextExtensions
    {
        public static Solicitante GetUser(this IHttpContextAccessor self)
        {
            if (self.HttpContext?.User == null)
                throw new InvalidOperationException("Não existe um usuário autenticado");

            try
            {
                return new Solicitante(self.HttpContext?.User);
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException("Erro ao recuperar solicitante a partir do contexto Http", ex);
            }
        }

        public static bool HasAuthorizedUser(this HttpContext self)
        {
            if (!self.User.HasClaim(c=> c.Type == Configuracoes.ClaimNames.UserId))
                return false;
            
            return true;
        }
    }
}
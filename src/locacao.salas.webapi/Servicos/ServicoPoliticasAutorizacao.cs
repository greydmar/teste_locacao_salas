﻿using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using mtgroup.auth.Interfaces;

namespace mtgroup.locacao.Servicos
{
    using static Constantes.NomesPoliticasAutorizacao;
    
    public sealed class Constantes
    {
        public static class NomesPoliticasAutorizacao
        {
            public const string UsuarioAutenticadoJwt = nameof(UsuarioAutenticadoJwt);
            public const string ApenasUsuariosAutenticados = nameof(ApenasUsuariosAutenticados);
        }
    }
    
    internal static class PoliticasAutorizacao
    {
        public static void AddPolicyUsuarioAutenticado(this AuthorizationOptions self)
        {
            var usuarioAutenticadoJwt = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();

            self.AddPolicy(UsuarioAutenticadoJwt, usuarioAutenticadoJwt);
            self.AddPolicy(ApenasUsuariosAutenticados, ChecarUsuarioAutenticado);
        }

        private static void ChecarUsuarioAutenticado(AuthorizationPolicyBuilder builder)
        {
            builder.RequireAssertion(context => context.User.HasClaim(
                claim => claim.Type == Configuracoes.ClaimNames.UserId
                         && int.TryParse(claim.Value, out var result) && result > 0
            ));
        }
    }
}

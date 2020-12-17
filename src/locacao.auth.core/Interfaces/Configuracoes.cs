using System;
using System.Collections.Immutable;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace mtgroup.auth.Interfaces
{
    public class Configuracoes
    {
        public class Auth
        {
            //TODO: Movimentar para configurações
            public const string AppSecret = "AgenciaZetta testando candidatos para d353nv01Dor .Net";
            private static readonly byte[] AppSecretBytes = Encoding.ASCII.GetBytes(AppSecret);

            public const string ContextUserKeyName = "AuthorizedUser";
            
            public static SecurityKey NewSymmetricSecurityKey => new SymmetricSecurityKey(AppSecretBytes);

            public static TokenValidationParameters ValidationParameters => new TokenValidationParameters()
            {
                ValidateLifetime = true,
                ValidateIssuer = false, /* Ignorado para este teste*/
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = NewSymmetricSecurityKey,
            };
        }

        public class ClaimNames
        {
            internal const string ClaimTypeNamespace = "http://agzetta.teste.com/ws/2020/125/identity/claims";

            public const string UserId = ClaimTypeNamespace + "/user-id";
            public const string UserLogin = ClaimTypeNamespace + "/user-login";
        }
        
        
    }
}
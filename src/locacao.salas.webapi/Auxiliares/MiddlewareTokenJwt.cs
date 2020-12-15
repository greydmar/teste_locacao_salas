using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using locacao.auth.core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace mtgroup.locacaosalas.Auxiliares
{
    public class MiddlewareTokenJwt
    {
        private readonly RequestDelegate _next;

        public MiddlewareTokenJwt(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"]
                .FirstOrDefault()?.Split(" ")
                .Last();

            if (token != null)
                EnsureUserContext(context, token);

            await _next(context);
        }

        private void EnsureUserContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(Configuracoes.Auth.AppSecret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken) validatedToken;
                
                var claimUserId = jwtToken.Claims
                    .FirstOrDefault(claim => claim.Type == Configuracoes.ClaimNames.UserId);

                var claimUserName = jwtToken.Claims
                    .FirstOrDefault(claim => claim.Type == Configuracoes.ClaimNames.UserLogin);

                var userId = int.Parse(claimUserId?.Value ?? "");
                var userLogin = claimUserName?.Value;
                
                // attach user to context on successful jwt validation
                context.User = new ClaimsPrincipal(new ClaimsIdentity(new[] {claimUserId, claimUserName}));
                //context.Items[Configuracoes.Auth.ContextUserKeyName] = new ClaimsPrincipal();
            }
            catch
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }
    }
}
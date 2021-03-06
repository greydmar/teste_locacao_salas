using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using mtgroup.auth.Interfaces;

namespace mtgroup.locacao.Auxiliares
{
    internal static class ConfigSerializacaoJson
    {
        public static void Setup(JsonSerializerOptions serializerOptions)
        {
            serializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            serializerOptions.PropertyNameCaseInsensitive = true;
            serializerOptions.IgnoreNullValues = true;
            serializerOptions.Converters.Add(new SimpleTimeSpanConverter());

        }

        public class SimpleTimeSpanConverter : JsonConverter<TimeSpan>
        {
            public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var rawData = reader.GetString();
                if (string.IsNullOrEmpty(rawData))
                    return default;

                return TimeSpan.Parse(rawData, CultureInfo.InvariantCulture);
            }

            public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString("c", CultureInfo.InvariantCulture));
            }
        }
    }


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
                tokenHandler.ValidateToken(token, Configuracoes.Auth.ValidationParameters, out SecurityToken validatedToken);

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
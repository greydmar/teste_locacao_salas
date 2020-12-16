using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using mtgroup.auth.DataModel;
using mtgroup.auth.Interfaces;
using mtgroup.auth.Interfaces.Model;

namespace mtgroup.auth.Servicos
{
    public class ServicoAutorizacaoUsuario : IServicoAutorizacao
    {
        private readonly IConsultaUsuarios _svcConsulta;

        public ServicoAutorizacaoUsuario(
            IConsultaUsuarios svcConsulta)
        {
            _svcConsulta = svcConsulta;
        }

        public async Task<Result<UsuarioAutorizado>> Autenticar(RequisicaoAutenticacaoUsuario model)
        {
            if (string.IsNullOrEmpty(model.Username))
                return Result.Fail("login é obrigatório");

            if (string.IsNullOrEmpty(model.Password))
                return Result.Fail("É necessário informar um password");

            var resultado = await _svcConsulta.Localizar(model);

            if (resultado == null)
                return Result.Fail(new UsuarioNaoAutorizado("Usuário não existe"));

            var result = PasswordHasher2.VerifyHashedPassword(resultado, model.Password);
            
            if (result != PasswordVerificationResult.Success)
                return Result.Fail(new UsuarioNaoAutorizado("Falha de credenciais. Erro de login/senha"));

            return Result.Ok(PrepararResposta(resultado));
        }

        private UsuarioAutorizado PrepararResposta(Usuario usuario)
        {
            // Gera um token válido por 50 minutos
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Configuracoes.Auth.AppSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(Configuracoes.ClaimNames.UserId, usuario.Id.ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Integer32),
                    new Claim(Configuracoes.ClaimNames.UserLogin, usuario.NomeLogin, ClaimValueTypes.String)
                }),
                
                Expires = DateTime.UtcNow.AddMinutes(50),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var resultToken = tokenHandler.WriteToken(token);

            return new UsuarioAutorizado()
            {
                NomeUsuario = usuario.NomeLogin,
                TempoVidaToken = 50,
                Token = resultToken
            };
        }
    }
}
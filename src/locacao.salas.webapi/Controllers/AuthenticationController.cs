using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtgroup.auth.Interfaces;
using mtgroup.auth.Interfaces.Model;

namespace mtgroup.locacao.Controllers
{
    using static Microsoft.AspNetCore.Http.StatusCodes;
    
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IServicoAutorizacao _svcAutorizacao;

        public AuthenticationController(IServicoAutorizacao svcAutorizacao)
        {
            _svcAutorizacao = svcAutorizacao;
        }

        
        [HttpPost("authenticate")]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status401Unauthorized)]
        [ProducesResponseType(typeof(UsuarioAutorizado), Status200OK)]
        public async Task<IActionResult> AuthByCredentials([FromBody] RequisicaoAutenticacaoUsuario requisicao)
        {
            var resposta = await _svcAutorizacao.Autenticar(requisicao);

            if (resposta.IsSuccess)
            {
                var autorizacao = resposta.Value;
                return Ok(autorizacao);
            }

            var nAutorizado = resposta.Errors.OfType<UsuarioNaoAutorizado>();
            if (nAutorizado.Any())
            {
                return Unauthorized(nAutorizado.FirstOrDefault());
            }

            var outros = resposta.Errors;

            if (outros.Any())
                return BadRequest(outros.FirstOrDefault());

            return StatusCode(Status500InternalServerError);
        }
    }
}

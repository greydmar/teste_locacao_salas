using System.ComponentModel.DataAnnotations;

namespace locacao.auth.core.DataModel
{
    public class RequisicaoAutenticacaoUsuario
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
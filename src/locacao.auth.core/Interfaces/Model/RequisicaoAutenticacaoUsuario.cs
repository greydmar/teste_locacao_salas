using System.ComponentModel.DataAnnotations;

namespace mtgroup.auth.Interfaces.Model
{
    public class RequisicaoAutenticacaoUsuario
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
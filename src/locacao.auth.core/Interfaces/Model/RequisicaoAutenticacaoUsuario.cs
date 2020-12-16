using System.ComponentModel.DataAnnotations;

namespace mtgroup.auth.Interfaces.Model
{
    public class RequisicaoAutenticacaoUsuario
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Senha { get; set; }
    }
}
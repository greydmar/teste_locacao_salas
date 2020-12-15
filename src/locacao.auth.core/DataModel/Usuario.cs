using System.Text.Json.Serialization;
using locacao.auth.core.Interfaces;

namespace locacao.auth.core.DataModel
{
    public class Usuario : IEntidade
    {
        public Usuario(int id)
        {
            Id = id;
        }

        public int Id { get; }
        
        public string Nome { get; set; }
        public string SobreNome { get; set; }
        
        public string NomeLogin { get; set; }

        [JsonIgnore]
        public string Password { get; set; }
    }
}
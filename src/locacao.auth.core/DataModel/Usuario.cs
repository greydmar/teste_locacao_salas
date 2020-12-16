using System.Text.Json.Serialization;
using mtgroup.auth.Interfaces;

namespace mtgroup.auth.DataModel
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
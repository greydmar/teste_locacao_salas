namespace mtgroup.auth.Interfaces.Model
{
    public class RespostaUsuarioAutorizado
    {
        public string NomeUsuario { get; set; }
        public int TempoVidaToken { get; set; }
        public string Token { get; set; }
    }
}

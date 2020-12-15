namespace locacao.auth.core.DataModel
{
    public class UsuarioAutorizado
    {
        public string NomeUsuario { get; set; }
        public int TempoVidaToken { get; set; }
        public string Token { get; set; }
    }
}

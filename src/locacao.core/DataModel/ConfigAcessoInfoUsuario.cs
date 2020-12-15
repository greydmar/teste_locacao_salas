namespace mtgroup.locacao.DataModel
{
    internal class ConfigAcessoInfoUsuario
    {
        public class ClaimNames
        {
            internal const string ClaimTypeNamespace = "http://agzetta.teste.com/ws/2020/125/identity/claims";

            public const string UserId = ClaimTypeNamespace + "/user-id";
            public const string UserLogin = ClaimTypeNamespace + "/user-login";
        }
    }
}
namespace locacao.auth.core.Interfaces
{
    public class Configuracoes
    {
        public class Auth
        {
            //TODO: Movimentar para configurações
            public const string AppSecret = "AgenciaZetta testando candidatos para d353nv01Dor .Net";

            public const string ContextUserKeyName = "AuthorizedUser";
        }

        public class ClaimNames
        {
            internal const string ClaimTypeNamespace = "http://agzetta.teste.com/ws/2020/125/identity/claims";

            public const string UserId = ClaimTypeNamespace + "/user-id";
            public const string UserLogin = ClaimTypeNamespace + "/user-login";
        }
        
        
    }
}
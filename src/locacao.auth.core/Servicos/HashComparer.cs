using locacao.auth.core.DataModel;
using Microsoft.AspNetCore.Identity;

namespace locacao.auth.core.Servicos
{
    internal sealed class PasswordHasher2: PasswordHasher<Usuario>
    {
        public static string HashPassword(Usuario user)
        {
            var instance = new PasswordHasher2();
            return instance.HashPassword(user, user.Password);
        }

        public static PasswordVerificationResult VerifyHashedPassword(Usuario user, string testingPassword)
        {
            var instance = new PasswordHasher2();
            return instance.VerifyHashedPassword(user, user.Password, testingPassword);
        }
    }
}
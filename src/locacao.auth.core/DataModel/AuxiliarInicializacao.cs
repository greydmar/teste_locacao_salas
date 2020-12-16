using System;
using System.Collections.Generic;
using System.Linq;
using mtgroup.auth.Servicos;

namespace mtgroup.auth.DataModel
{
    internal static class AuxiliarInicializacao
    {
        private static readonly Lazy<IEnumerable<Usuario>> _lzyUsuarios =
            new Lazy<IEnumerable<Usuario>>(() =>
                new[]
                {
                    new Usuario(0) {Nome = "Usuario01", NomeLogin = "Usuario01", Password = "Mudar@123"},
                    new Usuario(0) {Nome = "Usuario02", NomeLogin = "Usuario02", Password = "Mudar@123"},
                    new Usuario(0) {Nome = "Usuario03", NomeLogin = "Usuario03", Password = "Mudar@123"},
                }
            );


        public static IEnumerable<Usuario> UsuariosAmostra
        {
            get
            {
                return _lzyUsuarios.Value.Select((item, idx) =>
                    new Usuario(idx + 1)
                    {
                        NomeLogin = item.NomeLogin,
                        Nome = item.Nome,
                        SobreNome = item.SobreNome,
                        Password = PasswordHasher2.HashPassword(item)
                    });
            }
        } 
    }
}
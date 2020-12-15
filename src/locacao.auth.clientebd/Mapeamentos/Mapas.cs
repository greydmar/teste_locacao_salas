using locacao.auth.core.DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace mtgroup.auth.Mapeamentos
{
    internal class UsuarioConfiguracao : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("sistema_usuario_conectado");
            builder.HasKey(p => p.Id);
            builder.HasIndex(p => p.Id)
                .IsUnique()
                .HasDatabaseName("sistema_usuario_identificador");

            builder.HasIndex(o => o.NomeLogin)
                .IsUnique()
                .HasDatabaseName("sistema_usuario_nome");

            builder.Property(o=>o.Id)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();

            builder.Property(o => o.NomeLogin).HasMaxLength(30).IsRequired();
            builder.Property(o => o.Nome).HasMaxLength(30).IsRequired();
            builder.Property(o => o.SobreNome).HasMaxLength(100);
            builder.Property(o => o.Password).HasMaxLength(100).IsRequired();
        }
    }
}

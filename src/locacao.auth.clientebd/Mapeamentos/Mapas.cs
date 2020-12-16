using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using mtgroup.auth.DataModel;

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
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(o => o.NomeLogin).HasColumnName("nomeLogin")
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(o => o.Nome)
                .IsRequired().HasMaxLength(30);
            builder.Property(o => o.SobreNome).HasMaxLength(100);
            builder.Property(o => o.Password).HasMaxLength(100).IsRequired();

            builder.HasData(AuxiliarInicializacao.UsuariosAmostra);
        }
    }
}

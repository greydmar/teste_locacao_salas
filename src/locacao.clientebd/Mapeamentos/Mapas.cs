using System;
using System.Threading;
using locacao.clientebd.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using mtgroup.locacao.DataModel;

namespace locacao.clientebd.Mapeamentos
{
    internal class PerfilSalaReuniaoConfiguracao: IEntityTypeConfiguration<PerfilSalaReuniaoInterno>
    {
        public void Configure(EntityTypeBuilder<PerfilSalaReuniaoInterno> builder)
        {
            builder.ToTable("perfil_sala_reuniao");
            builder.HasKey(p => p.Id);
            builder.HasIndex(p => p.Identificador)
                .IsUnique()
                .HasDatabaseName("perfil_sala_identificador");

            builder.Property(o=>o.Id)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();

            builder.Property(o => o.Identificador).IsRequired();
            builder.Property(o => o.QuantidadeAssentos).IsRequired();
            builder.Property(o => o.Grupo).IsRequired();
        }
    }

    internal class ReservaSalaReuniaoConfiguracao : IEntityTypeConfiguration<ReservaSalaReuniao>
    {
        public void Configure(EntityTypeBuilder<ReservaSalaReuniao> builder)
        {
            builder.ToTable("reserva_sala_reuniao");
            builder.HasKey(p => p.Id);

            builder.HasIndex(o => o.CodigoReserva)
                .IsUnique()
                .HasDatabaseName("reserva_sala_codigo_reserva_unico");

            builder.Property(o => o.Id)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();

            builder.Property(o => o.CodigoReserva)
                .HasMaxLength(50)
                .IsRequired()
                .HasValueGenerator(typeof(GeradorCodigoReservas))
                .ValueGeneratedOnAdd();
            
            builder.Property(o => o.QuantidadePessoas).IsRequired();
            builder.Property(o => o.IdSalaReservada).IsRequired();
            builder.OwnsOne(o => o.Periodo, ba =>
                {
                    
                    ba.Property(p => p.Inicio).IsRequired();
                    ba.Property(p => p.Termino).IsRequired();
                    ba.Ignore(p=> p.Horas);
                });

            builder.Navigation(o=> o.Solicitante)
                .AutoInclude(true)
                .IsRequired();
        }
    }

    internal class SolicitanteConfiguracao : IEntityTypeConfiguration<Solicitante>
    {
        public void Configure(EntityTypeBuilder<Solicitante> builder)
        {
            builder.ToTable("sistema_usuario_conectado");
            builder.HasKey(o => o.Id);

            builder.HasIndex(o => o.Id)
                .IsUnique()
                .HasDatabaseName("sistema_usuario_identificador");

            builder.HasIndex(o => o.Name)
                .IsUnique()
                .HasDatabaseName("sistema_usuario_nome");

            builder.Property(o => o.Id)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();
            
            builder.Property(o => o.Name)
                .IsRequired()
                .HasMaxLength(50);
        }
    }

    internal class GeradorCodigoReservas : ValueGenerator<string>
    {
        private long _counter = DateTime.UtcNow.Ticks;
        public override bool GeneratesTemporaryValues => false;

        public override string Next(EntityEntry entry)
        {
            var result = new byte[8];
            var counterBytes = BitConverter.GetBytes(Interlocked.Increment(ref _counter));

            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(counterBytes);
            }

            result[00] = counterBytes[1];
            result[01] = counterBytes[0];
            result[02] = counterBytes[7];
            result[03] = counterBytes[6];
            result[04] = counterBytes[5];
            result[05] = counterBytes[4];
            result[06] = counterBytes[3];
            result[07] = counterBytes[2];

            var b64Result = Convert.ToBase64String(result, Base64FormattingOptions.None);
            return "zEt" + b64Result.Substring(0, b64Result.Length - 1);
        }
    }
}

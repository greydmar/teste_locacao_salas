using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace mtgroup.db.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "perfil_sala_reuniao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Grupo = table.Column<string>(type: "TEXT", nullable: false),
                    Identificador = table.Column<string>(type: "TEXT", nullable: false),
                    QuantidadeAssentos = table.Column<ushort>(type: "INTEGER", nullable: false),
                    Recursos = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_perfil_sala_reuniao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "reserva_sala_reuniao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdSalaReservada = table.Column<string>(type: "TEXT", nullable: false),
                    SolicitanteId = table.Column<int>(type: "INTEGER", nullable: false),
                    CodigoReserva = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Periodo_Inicio = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Periodo_Termino = table.Column<DateTime>(type: "TEXT", nullable: true),
                    QuantidadePessoas = table.Column<ushort>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reserva_sala_reuniao", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "perfil_sala_reuniao",
                columns: new[] { "Id", "Grupo", "Identificador", "QuantidadeAssentos", "Recursos" },
                values: new object[] { 1, "Grupo01", "01", (ushort)10, 7 });

            migrationBuilder.InsertData(
                table: "perfil_sala_reuniao",
                columns: new[] { "Id", "Grupo", "Identificador", "QuantidadeAssentos", "Recursos" },
                values: new object[] { 2, "Grupo01", "02", (ushort)10, 7 });

            migrationBuilder.InsertData(
                table: "perfil_sala_reuniao",
                columns: new[] { "Id", "Grupo", "Identificador", "QuantidadeAssentos", "Recursos" },
                values: new object[] { 3, "Grupo01", "03", (ushort)10, 7 });

            migrationBuilder.InsertData(
                table: "perfil_sala_reuniao",
                columns: new[] { "Id", "Grupo", "Identificador", "QuantidadeAssentos", "Recursos" },
                values: new object[] { 4, "Grupo01", "04", (ushort)10, 7 });

            migrationBuilder.InsertData(
                table: "perfil_sala_reuniao",
                columns: new[] { "Id", "Grupo", "Identificador", "QuantidadeAssentos", "Recursos" },
                values: new object[] { 5, "Grupo01", "05", (ushort)10, 7 });

            migrationBuilder.InsertData(
                table: "perfil_sala_reuniao",
                columns: new[] { "Id", "Grupo", "Identificador", "QuantidadeAssentos", "Recursos" },
                values: new object[] { 6, "Grupo02", "06", (ushort)10, 2 });

            migrationBuilder.InsertData(
                table: "perfil_sala_reuniao",
                columns: new[] { "Id", "Grupo", "Identificador", "QuantidadeAssentos", "Recursos" },
                values: new object[] { 7, "Grupo02", "07", (ushort)10, 2 });

            migrationBuilder.InsertData(
                table: "perfil_sala_reuniao",
                columns: new[] { "Id", "Grupo", "Identificador", "QuantidadeAssentos", "Recursos" },
                values: new object[] { 8, "Grupo03", "08", (ushort)3, 10 });

            migrationBuilder.InsertData(
                table: "perfil_sala_reuniao",
                columns: new[] { "Id", "Grupo", "Identificador", "QuantidadeAssentos", "Recursos" },
                values: new object[] { 9, "Grupo03", "09", (ushort)3, 10 });

            migrationBuilder.InsertData(
                table: "perfil_sala_reuniao",
                columns: new[] { "Id", "Grupo", "Identificador", "QuantidadeAssentos", "Recursos" },
                values: new object[] { 10, "Grupo03", "10", (ushort)3, 10 });

            migrationBuilder.InsertData(
                table: "perfil_sala_reuniao",
                columns: new[] { "Id", "Grupo", "Identificador", "QuantidadeAssentos", "Recursos" },
                values: new object[] { 11, "Grupo04", "11", (ushort)20, 0 });

            migrationBuilder.InsertData(
                table: "perfil_sala_reuniao",
                columns: new[] { "Id", "Grupo", "Identificador", "QuantidadeAssentos", "Recursos" },
                values: new object[] { 12, "Grupo04", "12", (ushort)20, 0 });

            migrationBuilder.CreateIndex(
                name: "perfil_sala_identificador",
                table: "perfil_sala_reuniao",
                column: "Identificador",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_reserva_sala_reuniao_SolicitanteId",
                table: "reserva_sala_reuniao",
                column: "SolicitanteId");

            migrationBuilder.CreateIndex(
                name: "reserva_sala_codigo_reserva_unico",
                table: "reserva_sala_reuniao",
                column: "CodigoReserva",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "perfil_sala_reuniao");

            migrationBuilder.DropTable(
                name: "reserva_sala_reuniao");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace mtgroup.db.Migrations.ContextoAutorizacaoMigrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sistema_usuario_conectado",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    SobreNome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    nomeLogin = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sistema_usuario_conectado", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "sistema_usuario_conectado",
                columns: new[] { "Id", "Nome", "nomeLogin", "Password", "SobreNome" },
                values: new object[] { 1, "Usuario01", "Usuario01", "AQAAAAEAACcQAAAAEAVDkh7tgSGNKE0PAgZge/EAL6OPvnE2MCN2swKjNtt3EGFa/Gvmlfigzqr/4AyMHw==", null });

            migrationBuilder.InsertData(
                table: "sistema_usuario_conectado",
                columns: new[] { "Id", "Nome", "nomeLogin", "Password", "SobreNome" },
                values: new object[] { 2, "Usuario02", "Usuario02", "AQAAAAEAACcQAAAAEPZSXLEYeiBQ1wN2eBUjCfPx1qe7z842MdtHVTuTqVVIraplnq7mkoE7DaVkHubjRw==", null });

            migrationBuilder.InsertData(
                table: "sistema_usuario_conectado",
                columns: new[] { "Id", "Nome", "nomeLogin", "Password", "SobreNome" },
                values: new object[] { 3, "Usuario03", "Usuario03", "AQAAAAEAACcQAAAAEGN34z8Msov6lRZvtYFPw6O3Fi3wigPbskyNkSKAgh4QpKiYVSm09N2aA1F/yufOUA==", null });

            migrationBuilder.CreateIndex(
                name: "sistema_usuario_identificador",
                table: "sistema_usuario_conectado",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "sistema_usuario_nome",
                table: "sistema_usuario_conectado",
                column: "nomeLogin",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sistema_usuario_conectado");
        }
    }
}

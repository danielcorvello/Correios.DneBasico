using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Correios.DneBasico.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateCepsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ceps",
                columns: table => new
                {
                    codigo = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    ibge = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    municipio = table.Column<string>(type: "character varying(72)", maxLength: 72, nullable: false),
                    uf = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    bairro = table.Column<string>(type: "character varying(72)", maxLength: 72, nullable: true),
                    distrito = table.Column<string>(type: "character varying(72)", maxLength: 72, nullable: true),
                    tipo_logradouro = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true),
                    logradouro = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    logradouro_completo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    complemento = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    unidade = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    geral = table.Column<bool>(type: "boolean", nullable: false),
                    tipo = table.Column<int>(type: "integer", nullable: false),
                    lat = table.Column<double>(type: "double precision", precision: 10, scale: 8, nullable: true),
                    lng = table.Column<double>(type: "double precision", precision: 11, scale: 8, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ceps", x => x.codigo);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ceps");
        }
    }
}

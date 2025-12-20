using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Correios.DneBasico.Data.Migrations
{
    /// <inheritdoc />
    public partial class NavigationPropertiesEnhanced : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_faixas_numericas_seccionamento_logradouros_log_nu",
                table: "faixas_numericas_seccionamento");

            migrationBuilder.DropPrimaryKey(
                name: "PK_faixas_numericas_seccionamento",
                table: "faixas_numericas_seccionamento");

            migrationBuilder.AddPrimaryKey(
                name: "PK_faixas_numericas_seccionamento",
                table: "faixas_numericas_seccionamento",
                columns: new[] { "log_nu", "sec_nu_ini", "sec_in_lado" });

            migrationBuilder.AddForeignKey(
                name: "FK_faixas_numericas_seccionamento_logradouros_log_nu",
                table: "faixas_numericas_seccionamento",
                column: "log_nu",
                principalTable: "logradouros",
                principalColumn: "log_nu",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_faixas_numericas_seccionamento_logradouros_log_nu",
                table: "faixas_numericas_seccionamento");

            migrationBuilder.DropPrimaryKey(
                name: "PK_faixas_numericas_seccionamento",
                table: "faixas_numericas_seccionamento");

            migrationBuilder.AddPrimaryKey(
                name: "PK_faixas_numericas_seccionamento",
                table: "faixas_numericas_seccionamento",
                column: "log_nu");

            migrationBuilder.AddForeignKey(
                name: "FK_faixas_numericas_seccionamento_logradouros_log_nu",
                table: "faixas_numericas_seccionamento",
                column: "log_nu",
                principalTable: "logradouros",
                principalColumn: "log_nu",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

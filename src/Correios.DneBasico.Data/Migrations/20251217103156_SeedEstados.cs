using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Correios.DneBasico.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedEstados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "estados",
                columns: new[] { "ufe_sg", "ufe_nu", "ufe_no" },
                values: new object[,]
                {
                    { "AC", "12", "Acre" },
                    { "AL", "27", "Alagoas" },
                    { "AM", "13", "Amazonas" },
                    { "AP", "16", "Amapá" },
                    { "BA", "29", "Bahia" },
                    { "CE", "23", "Ceará" },
                    { "DF", "53", "Distrito Federal" },
                    { "ES", "32", "Espírito Santo" },
                    { "GO", "52", "Goiás" },
                    { "MA", "21", "Maranhão" },
                    { "MG", "31", "Minas Gerais" },
                    { "MS", "50", "Mato Grosso do Sul" },
                    { "MT", "51", "Mato Grosso" },
                    { "PA", "15", "Pará" },
                    { "PB", "25", "Paraíba" },
                    { "PE", "26", "Pernambuco" },
                    { "PI", "22", "Piauí" },
                    { "PR", "41", "Paraná" },
                    { "RJ", "33", "Rio de Janeiro" },
                    { "RN", "24", "Rio Grande do Norte" },
                    { "RO", "11", "Rondônia" },
                    { "RR", "14", "Roraima" },
                    { "RS", "43", "Rio Grande do Sul" },
                    { "SC", "42", "Santa Catarina" },
                    { "SE", "28", "Sergipe" },
                    { "SP", "35", "São Paulo" },
                    { "TO", "17", "Tocantins" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "estados",
                keyColumn: "ufe_sg",
                keyValue: "AC");

            migrationBuilder.DeleteData(
                table: "estados",
                keyColumn: "ufe_sg",
                keyValue: "AL");

            migrationBuilder.DeleteData(
                table: "estados",
                keyColumn: "ufe_sg",
                keyValue: "AM");

            migrationBuilder.DeleteData(
                table: "estados",
                keyColumn: "ufe_sg",
                keyValue: "AP");

            migrationBuilder.DeleteData(
                table: "estados",
                keyColumn: "ufe_sg",
                keyValue: "BA");

            migrationBuilder.DeleteData(
                table: "estados",
                keyColumn: "ufe_sg",
                keyValue: "CE");

            migrationBuilder.DeleteData(
                table: "estados",
                keyColumn: "ufe_sg",
                keyValue: "DF");

            migrationBuilder.DeleteData(
                table: "estados",
                keyColumn: "ufe_sg",
                keyValue: "ES");

            migrationBuilder.DeleteData(
                table: "estados",
                keyColumn: "ufe_sg",
                keyValue: "GO");

            migrationBuilder.DeleteData(
                table: "estados",
                keyColumn: "ufe_sg",
                keyValue: "MA");

            migrationBuilder.DeleteData(
                table: "estados",
                keyColumn: "ufe_sg",
                keyValue: "MG");

            migrationBuilder.DeleteData(
                table: "estados",
                keyColumn: "ufe_sg",
                keyValue: "MS");

            migrationBuilder.DeleteData(
                table: "estados",
                keyColumn: "ufe_sg",
                keyValue: "MT");

            migrationBuilder.DeleteData(
                table: "estados",
                keyColumn: "ufe_sg",
                keyValue: "PA");

            migrationBuilder.DeleteData(
                table: "estados",
                keyColumn: "ufe_sg",
                keyValue: "PB");

            migrationBuilder.DeleteData(
                table: "estados",
                keyColumn: "ufe_sg",
                keyValue: "PE");

            migrationBuilder.DeleteData(
                table: "estados",
                keyColumn: "ufe_sg",
                keyValue: "PI");

            migrationBuilder.DeleteData(
                table: "estados",
                keyColumn: "ufe_sg",
                keyValue: "PR");

            migrationBuilder.DeleteData(
                table: "estados",
                keyColumn: "ufe_sg",
                keyValue: "RJ");

            migrationBuilder.DeleteData(
                table: "estados",
                keyColumn: "ufe_sg",
                keyValue: "RN");

            migrationBuilder.DeleteData(
                table: "estados",
                keyColumn: "ufe_sg",
                keyValue: "RO");

            migrationBuilder.DeleteData(
                table: "estados",
                keyColumn: "ufe_sg",
                keyValue: "RR");

            migrationBuilder.DeleteData(
                table: "estados",
                keyColumn: "ufe_sg",
                keyValue: "RS");

            migrationBuilder.DeleteData(
                table: "estados",
                keyColumn: "ufe_sg",
                keyValue: "SC");

            migrationBuilder.DeleteData(
                table: "estados",
                keyColumn: "ufe_sg",
                keyValue: "SE");

            migrationBuilder.DeleteData(
                table: "estados",
                keyColumn: "ufe_sg",
                keyValue: "SP");

            migrationBuilder.DeleteData(
                table: "estados",
                keyColumn: "ufe_sg",
                keyValue: "TO");
        }
    }
}

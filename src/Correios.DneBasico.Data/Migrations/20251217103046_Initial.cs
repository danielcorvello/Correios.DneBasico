using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Correios.DneBasico.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "estados",
                columns: table => new
                {
                    ufe_sg = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    ufe_no = table.Column<string>(type: "character varying(19)", maxLength: 19, nullable: false),
                    ufe_nu = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_estados", x => x.ufe_sg);
                });

            migrationBuilder.CreateTable(
                name: "paises",
                columns: table => new
                {
                    pai_sg = table.Column<string>(type: "text", nullable: false),
                    pai_sg_alternativa = table.Column<string>(type: "text", nullable: false),
                    pai_no_portugues = table.Column<string>(type: "text", nullable: false),
                    pai_no_ingles = table.Column<string>(type: "text", nullable: false),
                    pai_no_frances = table.Column<string>(type: "text", nullable: false),
                    pai_abreviatura = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_paises", x => x.pai_sg);
                });

            migrationBuilder.CreateTable(
                name: "faixas_cep_estado",
                columns: table => new
                {
                    ufe_sg = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    ufe_cep_ini = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    ufe_cep_fim = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_faixas_cep_estado", x => new { x.ufe_sg, x.ufe_cep_ini });
                    table.ForeignKey(
                        name: "FK_faixas_cep_estado_estados_ufe_sg",
                        column: x => x.ufe_sg,
                        principalTable: "estados",
                        principalColumn: "ufe_sg",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "localidades",
                columns: table => new
                {
                    loc_nu = table.Column<int>(type: "integer", nullable: false),
                    ufe_sg = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    loc_no = table.Column<string>(type: "character varying(72)", maxLength: 72, nullable: false),
                    loc_cep = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true),
                    loc_in_sit = table.Column<int>(type: "integer", nullable: false),
                    loc_in_tipo_loc = table.Column<int>(type: "integer", nullable: false),
                    loc_nu_sub = table.Column<int>(type: "integer", nullable: true),
                    loc_no_abrev = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true),
                    mun_nu = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_localidades", x => x.loc_nu);
                    table.ForeignKey(
                        name: "FK_localidades_estados_ufe_sg",
                        column: x => x.ufe_sg,
                        principalTable: "estados",
                        principalColumn: "ufe_sg",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_localidades_localidades_loc_nu_sub",
                        column: x => x.loc_nu_sub,
                        principalTable: "localidades",
                        principalColumn: "loc_nu");
                });

            migrationBuilder.CreateTable(
                name: "bairros",
                columns: table => new
                {
                    bai_nu = table.Column<int>(type: "integer", nullable: false),
                    ufe_sg = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    loc_nu = table.Column<int>(type: "integer", nullable: false),
                    bai_no = table.Column<string>(type: "character varying(72)", maxLength: 72, nullable: false),
                    bai_no_abrev = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bairros", x => x.bai_nu);
                    table.ForeignKey(
                        name: "FK_bairros_localidades_loc_nu",
                        column: x => x.loc_nu,
                        principalTable: "localidades",
                        principalColumn: "loc_nu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "caixas_postais_comunitarias",
                columns: table => new
                {
                    cpc_nu = table.Column<int>(type: "integer", nullable: false),
                    ufe_sg = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    loc_nu = table.Column<int>(type: "integer", nullable: false),
                    cpc_no = table.Column<string>(type: "character varying(72)", maxLength: 72, nullable: false),
                    cpc_endereco = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    cep = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_caixas_postais_comunitarias", x => x.cpc_nu);
                    table.ForeignKey(
                        name: "FK_caixas_postais_comunitarias_localidades_loc_nu",
                        column: x => x.loc_nu,
                        principalTable: "localidades",
                        principalColumn: "loc_nu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "faixas_cep_localidade",
                columns: table => new
                {
                    loc_nu = table.Column<int>(type: "integer", nullable: false),
                    loc_cep_ini = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    loc_tipo_faixa = table.Column<int>(type: "integer", nullable: false),
                    loc_cep_fim = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_faixas_cep_localidade", x => new { x.loc_nu, x.loc_cep_ini, x.loc_tipo_faixa });
                    table.ForeignKey(
                        name: "FK_faixas_cep_localidade_localidades_loc_nu",
                        column: x => x.loc_nu,
                        principalTable: "localidades",
                        principalColumn: "loc_nu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "variacoes_localidade",
                columns: table => new
                {
                    loc_nu = table.Column<int>(type: "integer", nullable: false),
                    val_nu = table.Column<int>(type: "integer", nullable: false),
                    val_tx = table.Column<string>(type: "character varying(72)", maxLength: 72, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_variacoes_localidade", x => new { x.loc_nu, x.val_nu });
                    table.ForeignKey(
                        name: "FK_variacoes_localidade_localidades_loc_nu",
                        column: x => x.loc_nu,
                        principalTable: "localidades",
                        principalColumn: "loc_nu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "faixas_cep_bairro",
                columns: table => new
                {
                    bai_nu = table.Column<int>(type: "integer", nullable: false),
                    fcb_cep_ini = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    fcb_cep_fim = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_faixas_cep_bairro", x => new { x.bai_nu, x.fcb_cep_ini });
                    table.ForeignKey(
                        name: "FK_faixas_cep_bairro_bairros_bai_nu",
                        column: x => x.bai_nu,
                        principalTable: "bairros",
                        principalColumn: "bai_nu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "logradouros",
                columns: table => new
                {
                    log_nu = table.Column<int>(type: "integer", nullable: false),
                    ufe_sg = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    loc_nu = table.Column<int>(type: "integer", nullable: false),
                    bai_nu_ini = table.Column<int>(type: "integer", nullable: false),
                    log_no = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    log_complemento = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    cep = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    tlo_tx = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    log_sta_tlo = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    log_no_abrev = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_logradouros", x => x.log_nu);
                    table.ForeignKey(
                        name: "FK_logradouros_bairros_bai_nu_ini",
                        column: x => x.bai_nu_ini,
                        principalTable: "bairros",
                        principalColumn: "bai_nu",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_logradouros_localidades_loc_nu",
                        column: x => x.loc_nu,
                        principalTable: "localidades",
                        principalColumn: "loc_nu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "variacoes_bairro",
                columns: table => new
                {
                    bai_nu = table.Column<int>(type: "integer", nullable: false),
                    vdb_nu = table.Column<int>(type: "integer", nullable: false),
                    vdb_tx = table.Column<string>(type: "character varying(72)", maxLength: 72, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_variacoes_bairro", x => new { x.bai_nu, x.vdb_nu });
                    table.ForeignKey(
                        name: "FK_variacoes_bairro_bairros_bai_nu",
                        column: x => x.bai_nu,
                        principalTable: "bairros",
                        principalColumn: "bai_nu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "faixas_caixa_postal_comunitaria",
                columns: table => new
                {
                    cpc_nu = table.Column<int>(type: "integer", nullable: false),
                    cpc_inicial = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    cpc_final = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_faixas_caixa_postal_comunitaria", x => new { x.cpc_nu, x.cpc_inicial });
                    table.ForeignKey(
                        name: "FK_faixas_caixa_postal_comunitaria_caixas_postais_comunitarias~",
                        column: x => x.cpc_nu,
                        principalTable: "caixas_postais_comunitarias",
                        principalColumn: "cpc_nu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "faixas_numericas_seccionamento",
                columns: table => new
                {
                    log_nu = table.Column<int>(type: "integer", nullable: false),
                    sec_nu_ini = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    sec_nu_fim = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    sec_in_lado = table.Column<int>(type: "integer", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_faixas_numericas_seccionamento", x => x.log_nu);
                    table.ForeignKey(
                        name: "FK_faixas_numericas_seccionamento_logradouros_log_nu",
                        column: x => x.log_nu,
                        principalTable: "logradouros",
                        principalColumn: "log_nu",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "grandes_usuarios",
                columns: table => new
                {
                    gru_nu = table.Column<int>(type: "integer", nullable: false),
                    ufe_sg = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    loc_nu = table.Column<int>(type: "integer", nullable: false),
                    bai_nu = table.Column<int>(type: "integer", nullable: false),
                    log_nu = table.Column<int>(type: "integer", nullable: true),
                    gru_no = table.Column<string>(type: "character varying(72)", maxLength: 72, nullable: false),
                    gru_endereco = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    cep = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    gru_no_abrev = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_grandes_usuarios", x => x.gru_nu);
                    table.ForeignKey(
                        name: "FK_grandes_usuarios_bairros_bai_nu",
                        column: x => x.bai_nu,
                        principalTable: "bairros",
                        principalColumn: "bai_nu",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_grandes_usuarios_localidades_loc_nu",
                        column: x => x.loc_nu,
                        principalTable: "localidades",
                        principalColumn: "loc_nu",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_grandes_usuarios_logradouros_log_nu",
                        column: x => x.log_nu,
                        principalTable: "logradouros",
                        principalColumn: "log_nu");
                });

            migrationBuilder.CreateTable(
                name: "unidades_operacionais",
                columns: table => new
                {
                    uop_nu = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ufe_sg = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    loc_nu = table.Column<int>(type: "integer", nullable: false),
                    bai_nu = table.Column<int>(type: "integer", nullable: false),
                    log_nu = table.Column<int>(type: "integer", nullable: true),
                    uop_no = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    uop_endereco = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    cep = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    uop_in_cp = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    uop_no_abrev = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_unidades_operacionais", x => x.uop_nu);
                    table.ForeignKey(
                        name: "FK_unidades_operacionais_bairros_bai_nu",
                        column: x => x.bai_nu,
                        principalTable: "bairros",
                        principalColumn: "bai_nu",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_unidades_operacionais_estados_ufe_sg",
                        column: x => x.ufe_sg,
                        principalTable: "estados",
                        principalColumn: "ufe_sg",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_unidades_operacionais_localidades_loc_nu",
                        column: x => x.loc_nu,
                        principalTable: "localidades",
                        principalColumn: "loc_nu",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_unidades_operacionais_logradouros_log_nu",
                        column: x => x.log_nu,
                        principalTable: "logradouros",
                        principalColumn: "log_nu");
                });

            migrationBuilder.CreateTable(
                name: "variacoes_logradouro",
                columns: table => new
                {
                    log_nu = table.Column<int>(type: "integer", nullable: false),
                    vlo_nu = table.Column<int>(type: "integer", nullable: false),
                    tlo_tx = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    vlo_tx = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_variacoes_logradouro", x => new { x.log_nu, x.vlo_nu });
                    table.ForeignKey(
                        name: "FK_variacoes_logradouro_logradouros_log_nu",
                        column: x => x.log_nu,
                        principalTable: "logradouros",
                        principalColumn: "log_nu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "faixas_caixa_postal_uop",
                columns: table => new
                {
                    uop_nu = table.Column<int>(type: "integer", nullable: false),
                    fnc_inicial = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    fnc_final = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_faixas_caixa_postal_uop", x => new { x.uop_nu, x.fnc_inicial });
                    table.ForeignKey(
                        name: "FK_faixas_caixa_postal_uop_unidades_operacionais_uop_nu",
                        column: x => x.uop_nu,
                        principalTable: "unidades_operacionais",
                        principalColumn: "uop_nu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_bairros_loc_nu",
                table: "bairros",
                column: "loc_nu");

            migrationBuilder.CreateIndex(
                name: "IX_caixas_postais_comunitarias_loc_nu",
                table: "caixas_postais_comunitarias",
                column: "loc_nu");

            migrationBuilder.CreateIndex(
                name: "IX_grandes_usuarios_bai_nu",
                table: "grandes_usuarios",
                column: "bai_nu");

            migrationBuilder.CreateIndex(
                name: "IX_grandes_usuarios_loc_nu",
                table: "grandes_usuarios",
                column: "loc_nu");

            migrationBuilder.CreateIndex(
                name: "IX_grandes_usuarios_log_nu",
                table: "grandes_usuarios",
                column: "log_nu");

            migrationBuilder.CreateIndex(
                name: "IX_localidades_loc_nu_sub",
                table: "localidades",
                column: "loc_nu_sub");

            migrationBuilder.CreateIndex(
                name: "IX_localidades_ufe_sg",
                table: "localidades",
                column: "ufe_sg");

            migrationBuilder.CreateIndex(
                name: "IX_logradouros_bai_nu_ini",
                table: "logradouros",
                column: "bai_nu_ini");

            migrationBuilder.CreateIndex(
                name: "IX_logradouros_loc_nu",
                table: "logradouros",
                column: "loc_nu");

            migrationBuilder.CreateIndex(
                name: "IX_unidades_operacionais_bai_nu",
                table: "unidades_operacionais",
                column: "bai_nu");

            migrationBuilder.CreateIndex(
                name: "IX_unidades_operacionais_loc_nu",
                table: "unidades_operacionais",
                column: "loc_nu");

            migrationBuilder.CreateIndex(
                name: "IX_unidades_operacionais_log_nu",
                table: "unidades_operacionais",
                column: "log_nu");

            migrationBuilder.CreateIndex(
                name: "IX_unidades_operacionais_ufe_sg",
                table: "unidades_operacionais",
                column: "ufe_sg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "faixas_caixa_postal_comunitaria");

            migrationBuilder.DropTable(
                name: "faixas_caixa_postal_uop");

            migrationBuilder.DropTable(
                name: "faixas_cep_bairro");

            migrationBuilder.DropTable(
                name: "faixas_cep_estado");

            migrationBuilder.DropTable(
                name: "faixas_cep_localidade");

            migrationBuilder.DropTable(
                name: "faixas_numericas_seccionamento");

            migrationBuilder.DropTable(
                name: "grandes_usuarios");

            migrationBuilder.DropTable(
                name: "paises");

            migrationBuilder.DropTable(
                name: "variacoes_bairro");

            migrationBuilder.DropTable(
                name: "variacoes_localidade");

            migrationBuilder.DropTable(
                name: "variacoes_logradouro");

            migrationBuilder.DropTable(
                name: "caixas_postais_comunitarias");

            migrationBuilder.DropTable(
                name: "unidades_operacionais");

            migrationBuilder.DropTable(
                name: "logradouros");

            migrationBuilder.DropTable(
                name: "bairros");

            migrationBuilder.DropTable(
                name: "localidades");

            migrationBuilder.DropTable(
                name: "estados");
        }
    }
}

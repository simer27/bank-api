using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Questao5Web.Migrations
{
    /// <inheritdoc />
    public partial class CreateBank : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContaCorrentes",
                columns: table => new
                {
                    IdContaCorrente = table.Column<string>(type: "TEXT", nullable: false),
                    Numero = table.Column<int>(type: "INTEGER", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContaCorrentes", x => x.IdContaCorrente);
                });

            migrationBuilder.CreateTable(
                name: "Idempotencias",
                columns: table => new
                {
                    ChaveIdempotencia = table.Column<string>(type: "TEXT", nullable: false),
                    Requisicao = table.Column<string>(type: "TEXT", nullable: false),
                    Resultado = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Idempotencias", x => x.ChaveIdempotencia);
                });

            migrationBuilder.CreateTable(
                name: "Movimentos",
                columns: table => new
                {
                    IdMovimento = table.Column<string>(type: "TEXT", nullable: false),
                    IdContaCorrente = table.Column<string>(type: "TEXT", nullable: false),
                    DataMovimento = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TipoMovimento = table.Column<string>(type: "TEXT", nullable: false),
                    Valor = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movimentos", x => x.IdMovimento);
                    table.ForeignKey(
                        name: "FK_Movimentos_ContaCorrentes_IdContaCorrente",
                        column: x => x.IdContaCorrente,
                        principalTable: "ContaCorrentes",
                        principalColumn: "IdContaCorrente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ContaCorrentes",
                columns: new[] { "IdContaCorrente", "Ativo", "Nome", "Numero" },
                values: new object[,]
                {
                    { "382D323D-7067-ED11-8866-7D5DFA4A16C9", true, "Tevin Mcconnell", 789 },
                    { "B6BAFC09-6967-ED11-A567-055DFA4A16C9", true, "Katherine Sanchez", 123 },
                    { "BCDACA4A-7067-ED11-AF81-825DFA4A16C9", false, "Jarrad Mckee", 852 },
                    { "D2E02051-7067-ED11-94C0-835DFA4A16C9", false, "Elisha Simons", 963 },
                    { "F475F943-7067-ED11-A06B-7E5DFA4A16C9", false, "Ameena Lynn", 741 },
                    { "FA99D033-7067-ED11-96C6-7C5DFA4A16C9", true, "Eva Woodward", 456 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movimentos_IdContaCorrente",
                table: "Movimentos",
                column: "IdContaCorrente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Idempotencias");

            migrationBuilder.DropTable(
                name: "Movimentos");

            migrationBuilder.DropTable(
                name: "ContaCorrentes");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gestão_de_projetos.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarValorProposta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Valor",
                table: "Propostas",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Valor",
                table: "Propostas");
        }
    }
}

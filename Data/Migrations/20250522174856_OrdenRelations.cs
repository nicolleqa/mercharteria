using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mercharteria.Data.Migrations
{
    /// <inheritdoc />
    public partial class OrdenRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DatosClienteId",
                table: "t_orden",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_t_orden_DatosClienteId",
                table: "t_orden",
                column: "DatosClienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_t_orden_t_DatosCliente_DatosClienteId",
                table: "t_orden",
                column: "DatosClienteId",
                principalTable: "t_DatosCliente",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_t_orden_t_DatosCliente_DatosClienteId",
                table: "t_orden");

            migrationBuilder.DropIndex(
                name: "IX_t_orden_DatosClienteId",
                table: "t_orden");

            migrationBuilder.DropColumn(
                name: "DatosClienteId",
                table: "t_orden");
        }
    }
}

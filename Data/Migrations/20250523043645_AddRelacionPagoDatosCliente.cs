using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mercharteria.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRelacionPagoDatosCliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DatosClienteId",
                table: "t_pago",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_t_pago_DatosClienteId",
                table: "t_pago",
                column: "DatosClienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_t_pago_t_DatosCliente_DatosClienteId",
                table: "t_pago",
                column: "DatosClienteId",
                principalTable: "t_DatosCliente",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_t_pago_t_DatosCliente_DatosClienteId",
                table: "t_pago");

            migrationBuilder.DropIndex(
                name: "IX_t_pago_DatosClienteId",
                table: "t_pago");

            migrationBuilder.DropColumn(
                name: "DatosClienteId",
                table: "t_pago");
        }
    }
}

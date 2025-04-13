using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mercharteria.Data.Migrations
{
    /// <inheritdoc />
    public partial class PreOrdenMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "t_pago",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FechaPago = table.Column<DateTime>(type: "TEXT", nullable: false),
                    NombreTarjeta = table.Column<string>(type: "TEXT", nullable: true),
                    NumeroTarjeta = table.Column<string>(type: "TEXT", nullable: true),
                    MontoTotal = table.Column<decimal>(type: "TEXT", nullable: false),
                    Estado = table.Column<string>(type: "TEXT", nullable: true),
                    UserName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_pago", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "t_preorden",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserName = table.Column<string>(type: "TEXT", nullable: true),
                    ProductoId = table.Column<int>(type: "INTEGER", nullable: true),
                    Cantidad = table.Column<int>(type: "INTEGER", nullable: false),
                    Precio = table.Column<string>(type: "TEXT", nullable: false),
                    Estado = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_preorden", x => x.Id);
                    table.ForeignKey(
                        name: "FK_t_preorden_t_producto_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "t_producto",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "t_orden",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserName = table.Column<string>(type: "TEXT", nullable: true),
                    Total = table.Column<decimal>(type: "TEXT", nullable: false),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PagoId = table.Column<int>(type: "INTEGER", nullable: true),
                    Estado = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_orden", x => x.id);
                    table.ForeignKey(
                        name: "FK_t_orden_t_pago_PagoId",
                        column: x => x.PagoId,
                        principalTable: "t_pago",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_t_orden_PagoId",
                table: "t_orden",
                column: "PagoId");

            migrationBuilder.CreateIndex(
                name: "IX_t_preorden_ProductoId",
                table: "t_preorden",
                column: "ProductoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_orden");

            migrationBuilder.DropTable(
                name: "t_preorden");

            migrationBuilder.DropTable(
                name: "t_pago");
        }
    }
}

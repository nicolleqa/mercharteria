using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mercharteria.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPersonajesAndRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductoPersonajes",
                columns: table => new
                {
                    ProductoId = table.Column<int>(type: "integer", nullable: false),
                    PersonajeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductoPersonajes", x => new { x.ProductoId, x.PersonajeId });
                    table.ForeignKey(
                        name: "FK_ProductoPersonajes_Personajes_PersonajeId",
                        column: x => x.PersonajeId,
                        principalTable: "Personajes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductoPersonajes_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductoPersonajes_PersonajeId",
                table: "ProductoPersonajes",
                column: "PersonajeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductoPersonajes");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mercharteria.Data.Migrations
{
    /// <inheritdoc />
    public partial class MLContactoPostgressMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Etiqueta",
                table: "Contactos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Puntuacion",
                table: "Contactos",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Etiqueta",
                table: "Contactos");

            migrationBuilder.DropColumn(
                name: "Puntuacion",
                table: "Contactos");
        }
    }
}

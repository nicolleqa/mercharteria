using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mercharteria.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSpotifyPlaylistId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SpotifyPlaylistId",
                table: "Personajes",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpotifyPlaylistId",
                table: "Personajes");
        }
    }
}

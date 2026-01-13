using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootLab.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddRegionToLeague : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Leagues",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Region",
                table: "Leagues");
        }
    }
}

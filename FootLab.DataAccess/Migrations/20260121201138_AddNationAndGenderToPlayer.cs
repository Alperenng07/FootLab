using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootLab.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddNationAndGenderToPlayer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "Players",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "NationId",
                table: "Players",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Nation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nation", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Players_NationId",
                table: "Players",
                column: "NationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Nation_NationId",
                table: "Players",
                column: "NationId",
                principalTable: "Nation",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Nation_NationId",
                table: "Players");

            migrationBuilder.DropTable(
                name: "Nation");

            migrationBuilder.DropIndex(
                name: "IX_Players_NationId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "NationId",
                table: "Players");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoteProject.API.Migrations
{
    /// <inheritdoc />
    public partial class add_NoteTable3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoteItself",
                table: "Notes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "NoteItself",
                table: "Notes",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}

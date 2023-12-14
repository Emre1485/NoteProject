using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoteProject.API.Migrations
{
    /// <inheritdoc />
    public partial class ad_NoteTable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NoteFilePath",
                table: "Notes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "NoteItself",
                table: "Notes",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoteFilePath",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "NoteItself",
                table: "Notes");
        }
    }
}

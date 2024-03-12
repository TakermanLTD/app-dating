using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Takerman.Dating.Data.Migrations
{
    /// <inheritdoc />
    public partial class PictureUrlsInDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "UserPictures",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "DatePictures",
                newName: "Url");

            migrationBuilder.AddColumn<string>(
                name: "PublicId",
                table: "UserPictures",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PublicId",
                table: "DatePictures",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicId",
                table: "UserPictures");

            migrationBuilder.DropColumn(
                name: "PublicId",
                table: "DatePictures");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "UserPictures",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "DatePictures",
                newName: "Name");
        }
    }
}

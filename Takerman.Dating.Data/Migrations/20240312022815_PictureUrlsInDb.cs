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
                table: "EthnicPictures",
                newName: "Url");

            migrationBuilder.AddColumn<string>(
                name: "PublicId",
                table: "UserPictures",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PublicId",
                table: "EthnicPictures",
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
                table: "EthnicPictures");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "UserPictures",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "EthnicPictures",
                newName: "Name");
        }
    }
}

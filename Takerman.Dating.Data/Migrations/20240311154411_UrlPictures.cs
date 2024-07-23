using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Takerman.Dating.Data.Migrations
{
    /// <inheritdoc />
    public partial class UrlPictures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Data",
                table: "UserPictures",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "Data",
                table: "EthnicPictures",
                newName: "Url");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                table: "UserPictures",
                newName: "Data");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "EthnicPictures",
                newName: "Data");
        }
    }
}

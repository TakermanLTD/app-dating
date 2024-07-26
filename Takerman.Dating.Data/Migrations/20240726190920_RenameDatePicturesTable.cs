using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Takerman.Dating.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameDatePicturesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EthnicPictures",
                table: "EthnicPictures");

            migrationBuilder.DropColumn(
                name: "Ethnicity",
                table: "Dates");

            migrationBuilder.DropColumn(
                name: "Ethnicity",
                table: "EthnicPictures");

            migrationBuilder.RenameTable(
                name: "EthnicPictures",
                newName: "DatePictures");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DatePictures",
                table: "DatePictures",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DatePictures",
                table: "DatePictures");

            migrationBuilder.RenameTable(
                name: "DatePictures",
                newName: "EthnicPictures");

            migrationBuilder.AddColumn<int>(
                name: "Ethnicity",
                table: "Dates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Ethnicity",
                table: "EthnicPictures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EthnicPictures",
                table: "EthnicPictures",
                column: "Id");
        }
    }
}

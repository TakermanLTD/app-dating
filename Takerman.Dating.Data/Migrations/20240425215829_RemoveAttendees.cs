using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Takerman.Dating.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAttendees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Dates_DateId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_DateId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DateId",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DateId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_DateId",
                table: "Users",
                column: "DateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Dates_DateId",
                table: "Users",
                column: "DateId",
                principalTable: "Dates",
                principalColumn: "Id");
        }
    }
}

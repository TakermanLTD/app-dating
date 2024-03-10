using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Takerman.Dating.Data.Migrations
{
    /// <inheritdoc />
    public partial class SetUserAvatar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Picture",
                table: "UserPictures",
                newName: "Data");

            migrationBuilder.AddColumn<int>(
                name: "AvatarId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DatePictures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateId = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    UploadedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatePictures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DatePictures_Dates_DateId",
                        column: x => x.DateId,
                        principalTable: "Dates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DatePictures_DateId",
                table: "DatePictures",
                column: "DateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DatePictures");

            migrationBuilder.DropColumn(
                name: "AvatarId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Data",
                table: "UserPictures",
                newName: "Picture");
        }
    }
}

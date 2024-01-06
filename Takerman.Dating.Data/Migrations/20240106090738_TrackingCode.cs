﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Takerman.Dating.Data.Migrations
{
    /// <inheritdoc />
    public partial class TrackingCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TrackingCode",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrackingCode",
                table: "Orders");
        }
    }
}

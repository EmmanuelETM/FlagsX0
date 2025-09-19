using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlagsX0.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFlagEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedTimeUtc",
                table: "Flags",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Flags",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedTimeUtc",
                table: "Flags");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Flags");
        }
    }
}

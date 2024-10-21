using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DragonData.Migrations
{
    /// <inheritdoc />
    public partial class DataGuildUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<ulong>(
                name: "queryCatagoryId",
                table: "Guilds",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<string>(
                name: "queryCategoryName",
                table: "Guilds",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "queryCatagoryId",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "queryCategoryName",
                table: "Guilds");
        }
    }
}

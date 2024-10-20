using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DragonData.Migrations
{
    /// <inheritdoc />
    public partial class DataWatcherCahe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "posting",
                table: "watcherPosts",
                newName: "pause");

            migrationBuilder.AddColumn<int>(
                name: "interval",
                table: "watcherPosts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "interval",
                table: "watcherPosts");

            migrationBuilder.RenameColumn(
                name: "pause",
                table: "watcherPosts",
                newName: "posting");
        }
    }
}

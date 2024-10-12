using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DragonData.Migrations
{
    /// <inheritdoc />
    public partial class pause : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "posting",
                table: "watcherPosts",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "posting",
                table: "watcherPosts");
        }
    }
}

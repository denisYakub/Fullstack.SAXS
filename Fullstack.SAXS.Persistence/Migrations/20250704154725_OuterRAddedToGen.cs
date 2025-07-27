using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fullstack.SAXS.Server.Migrations
{
    /// <inheritdoc />
    public partial class OuterRAddedToGen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "area_outer_radius",
                table: "sp_generation",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "area_outer_radius",
                table: "sp_generation");
        }
    }
}

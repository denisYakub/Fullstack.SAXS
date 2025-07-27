using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fullstack.SAXS.Server.Migrations
{
    /// <inheritdoc />
    public partial class MorePages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "phi",
                table: "sp_generation",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "phi",
                table: "sp_generation",
                type: "real",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);
        }
    }
}

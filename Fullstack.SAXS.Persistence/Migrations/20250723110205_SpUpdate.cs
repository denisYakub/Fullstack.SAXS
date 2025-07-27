using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fullstack.SAXS.Server.Migrations
{
    /// <inheritdoc />
    public partial class SpUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sp_generation_sp_data_id_sp_data",
                table: "sp_generation");

            migrationBuilder.DropIndex(
                name: "IX_sp_generation_id_sp_data",
                table: "sp_generation");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_sp_generation_id_sp_data",
                table: "sp_generation",
                column: "id_sp_data",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_sp_generation_sp_data_id_sp_data",
                table: "sp_generation",
                column: "id_sp_data",
                principalTable: "sp_data",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

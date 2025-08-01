using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fullstack.SAXS.Server.Migrations
{
    /// <inheritdoc />
    public partial class ReworkedTables1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "id_sp_data",
                table: "sp_generation");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "sp_generation",
                newName: "id_generation");

            migrationBuilder.RenameColumn(
                name: "id_user",
                table: "sp_generation",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "sp_data",
                newName: "id_data");

            migrationBuilder.AddColumn<Guid>(
                name: "generation_id",
                table: "sp_data",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "sp_system_tasks",
                columns: table => new
                {
                    id_system_task = table.Column<Guid>(type: "uuid", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false),
                    area_size = table.Column<double>(type: "double precision", nullable: false),
                    area_copies_number = table.Column<int>(type: "integer", nullable: false),
                    area_type = table.Column<string>(type: "text", nullable: false),
                    particle_number = table.Column<int>(type: "integer", nullable: false),
                    particle_type = table.Column<string>(type: "text", nullable: false),
                    particle_min_size = table.Column<double>(type: "double precision", nullable: false),
                    particle_max_size = table.Column<double>(type: "double precision", nullable: false),
                    particle_size_shape = table.Column<double>(type: "double precision", nullable: false),
                    particle_size_scale = table.Column<double>(type: "double precision", nullable: false),
                    particle_alpha_rotation = table.Column<double>(type: "double precision", nullable: false),
                    particle_beta_rotation = table.Column<double>(type: "double precision", nullable: false),
                    particles_gamma_rotation = table.Column<double>(type: "double precision", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sp_system_tasks", x => x.id_system_task);
                });

            migrationBuilder.CreateIndex(
                name: "IX_sp_data_generation_id",
                table: "sp_data",
                column: "generation_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_sp_data_sp_generation_generation_id",
                table: "sp_data",
                column: "generation_id",
                principalTable: "sp_generation",
                principalColumn: "id_generation",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sp_data_sp_generation_generation_id",
                table: "sp_data");

            migrationBuilder.DropTable(
                name: "sp_system_tasks");

            migrationBuilder.DropIndex(
                name: "IX_sp_data_generation_id",
                table: "sp_data");

            migrationBuilder.DropColumn(
                name: "generation_id",
                table: "sp_data");

            migrationBuilder.RenameColumn(
                name: "id_generation",
                table: "sp_generation",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "sp_generation",
                newName: "id_user");

            migrationBuilder.RenameColumn(
                name: "id_data",
                table: "sp_data",
                newName: "id");

            migrationBuilder.AddColumn<Guid>(
                name: "id_sp_data",
                table: "sp_generation",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}

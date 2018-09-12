using Microsoft.EntityFrameworkCore.Migrations;

namespace DB.NetCore.Migrations
{
    public partial class FixMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Doctor_id",
                table: "VisitLogs");

            migrationBuilder.DropColumn(
                name: "Pacient_Id",
                table: "VisitLogs");

            migrationBuilder.DropColumn(
                name: "Document_Id",
                table: "Pacients");

            migrationBuilder.DropColumn(
                name: "Pacient_Id",
                table: "Documents");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Doctor_id",
                table: "VisitLogs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Pacient_Id",
                table: "VisitLogs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Document_Id",
                table: "Pacients",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Pacient_Id",
                table: "Documents",
                nullable: false,
                defaultValue: 0);
        }
    }
}

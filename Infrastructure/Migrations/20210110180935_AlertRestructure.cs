using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class AlertRestructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Alert",
                table: "Alert");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Alert",
                newName: "AlertId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Alert",
                table: "Alert",
                column: "AlertId");

            migrationBuilder.CreateIndex(
                name: "IX_Alert_ScheduleId",
                table: "Alert",
                column: "ScheduleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Alert",
                table: "Alert");

            migrationBuilder.DropIndex(
                name: "IX_Alert_ScheduleId",
                table: "Alert");

            migrationBuilder.RenameColumn(
                name: "AlertId",
                table: "Alert",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Alert",
                table: "Alert",
                columns: new[] { "ScheduleId", "Id" });
        }
    }
}

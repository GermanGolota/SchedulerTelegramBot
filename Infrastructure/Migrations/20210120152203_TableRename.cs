using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class TableRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alert_Schedules_ScheduleId",
                table: "Alert");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Alert",
                table: "Alert");

            migrationBuilder.RenameTable(
                name: "Alert",
                newName: "Alerts");

            migrationBuilder.RenameIndex(
                name: "IX_Alert_ScheduleId",
                table: "Alerts",
                newName: "IX_Alerts_ScheduleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Alerts",
                table: "Alerts",
                column: "AlertId");

            migrationBuilder.AddForeignKey(
                name: "FK_Alerts_Schedules_ScheduleId",
                table: "Alerts",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "ScheduleId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alerts_Schedules_ScheduleId",
                table: "Alerts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Alerts",
                table: "Alerts");

            migrationBuilder.RenameTable(
                name: "Alerts",
                newName: "Alert");

            migrationBuilder.RenameIndex(
                name: "IX_Alerts_ScheduleId",
                table: "Alert",
                newName: "IX_Alert_ScheduleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Alert",
                table: "Alert",
                column: "AlertId");

            migrationBuilder.AddForeignKey(
                name: "FK_Alert_Schedules_ScheduleId",
                table: "Alert",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "ScheduleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

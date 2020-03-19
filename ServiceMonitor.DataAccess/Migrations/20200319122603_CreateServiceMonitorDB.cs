using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ServiceMonitor.DataAccess.Migrations
{
    public partial class CreateServiceMonitorDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StatusLogs",
                columns: table => new
                {
                    StatusLogEntityId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceName = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Date = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusLogs", x => x.StatusLogEntityId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StatusLogs");
        }
    }
}

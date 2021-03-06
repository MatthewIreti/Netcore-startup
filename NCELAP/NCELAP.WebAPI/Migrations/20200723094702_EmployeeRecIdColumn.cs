using Microsoft.EntityFrameworkCore.Migrations;

namespace NCELAP.WebAPI.Migrations
{
    public partial class EmployeeRecIdColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "EmployeeRecId",
                table: "SupportTickets",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeRecId",
                table: "SupportTickets");
        }
    }
}

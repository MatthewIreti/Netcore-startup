using Microsoft.EntityFrameworkCore.Migrations;

namespace NCELAP.WebAPI.Migrations
{
    public partial class CreatedByUserInfoColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByUserRecId",
                table: "SupportTickets");

            migrationBuilder.DropColumn(
                name: "CustomerRecId",
                table: "SupportTickets");

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "SupportTickets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeEmail",
                table: "SupportTickets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeName",
                table: "SupportTickets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "SupportTickets");

            migrationBuilder.DropColumn(
                name: "EmployeeEmail",
                table: "SupportTickets");

            migrationBuilder.DropColumn(
                name: "EmployeeName",
                table: "SupportTickets");

            migrationBuilder.AddColumn<long>(
                name: "CreatedByUserRecId",
                table: "SupportTickets",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CustomerRecId",
                table: "SupportTickets",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}

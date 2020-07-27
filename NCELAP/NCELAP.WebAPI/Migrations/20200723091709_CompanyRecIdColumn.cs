using Microsoft.EntityFrameworkCore.Migrations;

namespace NCELAP.WebAPI.Migrations
{
    public partial class CompanyRecIdColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CompanyRecId",
                table: "SupportTickets",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyRecId",
                table: "SupportTickets");
        }
    }
}

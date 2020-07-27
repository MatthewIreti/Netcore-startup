using Microsoft.EntityFrameworkCore.Migrations;

namespace NCELAP.WebAPI.Migrations
{
    public partial class ContactEmailColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactEmail",
                table: "SupportTickets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactEmail",
                table: "SupportTickets");
        }
    }
}

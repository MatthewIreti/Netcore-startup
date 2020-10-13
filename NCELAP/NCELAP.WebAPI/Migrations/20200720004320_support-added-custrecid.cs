using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NCELAP.WebAPI.Migrations
{
    public partial class supportaddedcustrecid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CustomerRecId",
                table: "SupportTickets",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "RaisedOn",
                table: "SupportTickets",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerRecId",
                table: "SupportTickets");

            migrationBuilder.DropColumn(
                name: "RaisedOn",
                table: "SupportTickets");
        }
    }
}

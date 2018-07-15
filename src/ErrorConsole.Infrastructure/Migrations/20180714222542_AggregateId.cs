using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ErrorConsole.Infrastructure.Migrations
{
    public partial class AggregateId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AggregateId",
                table: "DomainEvents",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AggregateId",
                table: "DomainEvents");
        }
    }
}

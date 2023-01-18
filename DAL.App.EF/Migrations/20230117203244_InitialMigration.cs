using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.App.EF.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pilot",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    LastName = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Distance = table.Column<double>(type: "double precision", nullable: false),
                    PositionX = table.Column<double>(type: "double precision", nullable: false),
                    PositionY = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pilot", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pilot");
        }
    }
}

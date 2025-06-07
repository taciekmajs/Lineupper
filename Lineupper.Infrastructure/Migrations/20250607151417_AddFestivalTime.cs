using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lineupper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFestivalTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "ConcertEndTime",
                table: "Festivals",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ConcertStartTime",
                table: "Festivals",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConcertEndTime",
                table: "Festivals");

            migrationBuilder.DropColumn(
                name: "ConcertStartTime",
                table: "Festivals");
        }
    }
}

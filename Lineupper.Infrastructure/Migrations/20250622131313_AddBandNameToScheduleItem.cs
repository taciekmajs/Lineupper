using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lineupper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBandNameToScheduleItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BandName",
                table: "ScheduleItems",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BandName",
                table: "ScheduleItems");
        }
    }
}

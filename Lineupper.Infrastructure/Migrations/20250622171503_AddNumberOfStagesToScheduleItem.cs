using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lineupper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNumberOfStagesToScheduleItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StageNumber",
                table: "ScheduleItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StageNumber",
                table: "ScheduleItems");
        }
    }
}

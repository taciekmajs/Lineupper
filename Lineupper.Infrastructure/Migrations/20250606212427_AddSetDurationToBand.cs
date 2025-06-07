using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lineupper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSetDurationToBand : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SetDuration",
                table: "Bands",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SetDuration",
                table: "Bands");
        }
    }
}

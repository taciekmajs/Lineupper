using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lineupper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddValueToVote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Value",
                table: "Votes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "Votes");
        }
    }
}

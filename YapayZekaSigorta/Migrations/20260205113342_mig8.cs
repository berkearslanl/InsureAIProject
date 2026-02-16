using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YapayZekaSigorta.Migrations
{
    /// <inheritdoc />
    public partial class mig8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "PricingPlans",
                newName: "Description2");

            migrationBuilder.AddColumn<string>(
                name: "Description1",
                table: "PricingPlans",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description1",
                table: "PricingPlans");

            migrationBuilder.RenameColumn(
                name: "Description2",
                table: "PricingPlans",
                newName: "Description");
        }
    }
}

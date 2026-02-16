using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YapayZekaSigorta.Migrations
{
    /// <inheritdoc />
    public partial class mig14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PricingPlanItem_PricingPlans_PricingPlanId",
                table: "PricingPlanItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PricingPlanItem",
                table: "PricingPlanItem");

            migrationBuilder.RenameTable(
                name: "PricingPlanItem",
                newName: "PricingPlanItems");

            migrationBuilder.RenameIndex(
                name: "IX_PricingPlanItem_PricingPlanId",
                table: "PricingPlanItems",
                newName: "IX_PricingPlanItems_PricingPlanId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PricingPlanItems",
                table: "PricingPlanItems",
                column: "PricingPlanItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_PricingPlanItems_PricingPlans_PricingPlanId",
                table: "PricingPlanItems",
                column: "PricingPlanId",
                principalTable: "PricingPlans",
                principalColumn: "PricingPlanId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PricingPlanItems_PricingPlans_PricingPlanId",
                table: "PricingPlanItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PricingPlanItems",
                table: "PricingPlanItems");

            migrationBuilder.RenameTable(
                name: "PricingPlanItems",
                newName: "PricingPlanItem");

            migrationBuilder.RenameIndex(
                name: "IX_PricingPlanItems_PricingPlanId",
                table: "PricingPlanItem",
                newName: "IX_PricingPlanItem_PricingPlanId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PricingPlanItem",
                table: "PricingPlanItem",
                column: "PricingPlanItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_PricingPlanItem_PricingPlans_PricingPlanId",
                table: "PricingPlanItem",
                column: "PricingPlanId",
                principalTable: "PricingPlans",
                principalColumn: "PricingPlanId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

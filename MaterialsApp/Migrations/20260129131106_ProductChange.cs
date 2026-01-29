using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaterialsApp.Migrations
{
    /// <inheritdoc />
    public partial class ProductChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAssessments_Products_ProductId",
                table: "ProductAssessments");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ProductAssessments",
                newName: "OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductAssessments_ProductId",
                table: "ProductAssessments",
                newName: "IX_ProductAssessments_OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAssessments_Orders_OrderId",
                table: "ProductAssessments",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAssessments_Orders_OrderId",
                table: "ProductAssessments");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "ProductAssessments",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductAssessments_OrderId",
                table: "ProductAssessments",
                newName: "IX_ProductAssessments_ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAssessments_Products_ProductId",
                table: "ProductAssessments",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

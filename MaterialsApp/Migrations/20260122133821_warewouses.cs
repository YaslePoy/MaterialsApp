using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MaterialsApp.Migrations
{
    /// <inheritdoc />
    public partial class warewouses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WarehouseId",
                table: "Materials",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WarehouseId",
                table: "Accessories",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Warehouse",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouse", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Materials_WarehouseId",
                table: "Materials",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Accessories_WarehouseId",
                table: "Accessories",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accessories_Warehouse_WarehouseId",
                table: "Accessories",
                column: "WarehouseId",
                principalTable: "Warehouse",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_Warehouse_WarehouseId",
                table: "Materials",
                column: "WarehouseId",
                principalTable: "Warehouse",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accessories_Warehouse_WarehouseId",
                table: "Accessories");

            migrationBuilder.DropForeignKey(
                name: "FK_Materials_Warehouse_WarehouseId",
                table: "Materials");

            migrationBuilder.DropTable(
                name: "Warehouse");

            migrationBuilder.DropIndex(
                name: "IX_Materials_WarehouseId",
                table: "Materials");

            migrationBuilder.DropIndex(
                name: "IX_Accessories_WarehouseId",
                table: "Accessories");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "Accessories");
        }
    }
}

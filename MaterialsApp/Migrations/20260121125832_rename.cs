using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MaterialsApp.Migrations
{
    /// <inheritdoc />
    public partial class rename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductType",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "ProductType",
                table: "Accessories");

            migrationBuilder.AlterColumn<string>(
                name: "Patronymic",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "MaterialTypeId",
                table: "Materials",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AccessoryTypeId",
                table: "Accessories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AccessoryTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessoryTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaterialTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Materials_MaterialTypeId",
                table: "Materials",
                column: "MaterialTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Accessories_AccessoryTypeId",
                table: "Accessories",
                column: "AccessoryTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accessories_AccessoryTypes_AccessoryTypeId",
                table: "Accessories",
                column: "AccessoryTypeId",
                principalTable: "AccessoryTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_MaterialTypes_MaterialTypeId",
                table: "Materials",
                column: "MaterialTypeId",
                principalTable: "MaterialTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accessories_AccessoryTypes_AccessoryTypeId",
                table: "Accessories");

            migrationBuilder.DropForeignKey(
                name: "FK_Materials_MaterialTypes_MaterialTypeId",
                table: "Materials");

            migrationBuilder.DropTable(
                name: "AccessoryTypes");

            migrationBuilder.DropTable(
                name: "MaterialTypes");

            migrationBuilder.DropIndex(
                name: "IX_Materials_MaterialTypeId",
                table: "Materials");

            migrationBuilder.DropIndex(
                name: "IX_Accessories_AccessoryTypeId",
                table: "Accessories");

            migrationBuilder.DropColumn(
                name: "MaterialTypeId",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "AccessoryTypeId",
                table: "Accessories");

            migrationBuilder.AlterColumn<string>(
                name: "Patronymic",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductType",
                table: "Materials",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProductType",
                table: "Accessories",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}

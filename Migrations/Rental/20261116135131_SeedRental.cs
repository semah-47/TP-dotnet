using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ASPCoreFirstApp.Migrations.Rental
{
    /// <inheritdoc />
    public partial class SeedRental : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "rental",
                table: "Customers",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "rental",
                table: "Customers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MembershipTypeId",
                schema: "rental",
                table: "Customers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MembershipTypes",
                schema: "rental",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    DiscountPercent = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "rental",
                table: "MembershipTypes",
                columns: new[] { "Id", "DiscountPercent", "Name" },
                values: new object[,]
                {
                    { 1, 0, "Standard" },
                    { 2, 10, "Premium" },
                    { 3, 20, "Gold" }
                });

            migrationBuilder.InsertData(
                schema: "rental",
                table: "Customers",
                columns: new[] { "Id", "Email", "MembershipTypeId", "Name" },
                values: new object[,]
                {
                    { 1, "amal@example.com", 2, "Amal Ben Salah" },
                    { 2, "youssef@example.com", 1, "Youssef Trabelsi" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_MembershipTypeId",
                schema: "rental",
                table: "Customers",
                column: "MembershipTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_MembershipTypes_MembershipTypeId",
                schema: "rental",
                table: "Customers",
                column: "MembershipTypeId",
                principalSchema: "rental",
                principalTable: "MembershipTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_MembershipTypes_MembershipTypeId",
                schema: "rental",
                table: "Customers");

            migrationBuilder.DropTable(
                name: "MembershipTypes",
                schema: "rental");

            migrationBuilder.DropIndex(
                name: "IX_Customers_MembershipTypeId",
                schema: "rental",
                table: "Customers");

            migrationBuilder.DeleteData(
                schema: "rental",
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "rental",
                table: "Customers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "rental",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "MembershipTypeId",
                schema: "rental",
                table: "Customers");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "rental",
                table: "Customers",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 100);
        }
    }
}

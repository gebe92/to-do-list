using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class ChangeColumnType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "70f02d34-8082-4bef-8581-45614452547b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a2b3236f-25d6-4c33-bc86-303376d6653a");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TaskLists",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "34af7542-902c-459c-97d5-154d6f919258", null, "User", "USER" },
                    { "bb1ec1f1-312b-4868-815f-02176d3ada54", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "34af7542-902c-459c-97d5-154d6f919258");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bb1ec1f1-312b-4868-815f-02176d3ada54");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "TaskLists",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "70f02d34-8082-4bef-8581-45614452547b", null, "Admin", "ADMIN" },
                    { "a2b3236f-25d6-4c33-bc86-303376d6653a", null, "User", "USER" }
                });
        }
    }
}

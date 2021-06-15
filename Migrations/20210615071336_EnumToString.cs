using Microsoft.EntityFrameworkCore.Migrations;

namespace KingdomApi.Migrations
{
    public partial class EnumToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "action_level",
                table: "responsibilities",
                type: "varchar(24)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "gender",
                table: "noblemen",
                type: "varchar(24)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "action_level",
                table: "responsibilities",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(24)");

            migrationBuilder.AlterColumn<int>(
                name: "gender",
                table: "noblemen",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(24)");
        }
    }
}

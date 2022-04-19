using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectFora.Server.Migrations.AppDb
{
    public partial class updatedMessageModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasDeleted",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEdited",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasDeleted",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "IsEdited",
                table: "Messages");
        }
    }
}

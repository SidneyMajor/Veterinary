using Microsoft.EntityFrameworkCore.Migrations;

namespace Veterinary.Migrations
{
    public partial class ChangeNameColumnDocumentType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Document",
                table: "DocumentTypes");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "DocumentTypes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "DocumentTypes");

            migrationBuilder.AddColumn<string>(
                name: "Document",
                table: "DocumentTypes",
                nullable: false,
                defaultValue: "");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Veterinary.Migrations
{
    public partial class AddDocumentToClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FisrtName",
                table: "Clients",
                newName: "FirstName");

            migrationBuilder.AlterColumn<string>(
                name: "Document",
                table: "DocumentTypes",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Document",
                table: "Clients",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Document",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Clients",
                newName: "FisrtName");

            migrationBuilder.AlterColumn<string>(
                name: "Document",
                table: "DocumentTypes",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}

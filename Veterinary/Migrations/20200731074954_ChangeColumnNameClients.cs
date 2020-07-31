using Microsoft.EntityFrameworkCore.Migrations;

namespace Veterinary.Migrations
{
    public partial class ChangeColumnNameClients : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_DocumentTypes_DocumentTypeID",
                table: "Clients");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_DocumentTypes_DocumentTypeID",
                table: "Clients",
                column: "DocumentTypeID",
                principalTable: "DocumentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_DocumentTypes_DocumentTypeID",
                table: "Clients");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_DocumentTypes_DocumentTypeID",
                table: "Clients",
                column: "DocumentTypeID",
                principalTable: "DocumentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

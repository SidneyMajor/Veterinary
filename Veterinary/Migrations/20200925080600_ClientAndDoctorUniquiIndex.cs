using Microsoft.EntityFrameworkCore.Migrations;

namespace Veterinary.Migrations
{
    public partial class ClientAndDoctorUniquiIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SSNumber",
                table: "Doctors",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_Document",
                table: "Doctors",
                column: "Document",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_SSNumber",
                table: "Doctors",
                column: "SSNumber",
                unique: true,
                filter: "[SSNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_TaxNumber",
                table: "Doctors",
                column: "TaxNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Document",
                table: "Clients",
                column: "Document",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_TaxNumber",
                table: "Clients",
                column: "TaxNumber",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Doctors_Document",
                table: "Doctors");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_SSNumber",
                table: "Doctors");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_TaxNumber",
                table: "Doctors");

            migrationBuilder.DropIndex(
                name: "IX_Clients_Document",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_TaxNumber",
                table: "Clients");

            migrationBuilder.AlterColumn<string>(
                name: "SSNumber",
                table: "Doctors",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}

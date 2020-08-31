using Microsoft.EntityFrameworkCore.Migrations;

namespace Veterinary.Migrations
{
    public partial class addSpecialtyToAppointment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SpecialtyID",
                table: "Appointments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_SpecialtyID",
                table: "Appointments",
                column: "SpecialtyID");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Specialties_SpecialtyID",
                table: "Appointments",
                column: "SpecialtyID",
                principalTable: "Specialties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Specialties_SpecialtyID",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_SpecialtyID",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "SpecialtyID",
                table: "Appointments");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Veterinary.Migrations
{
    public partial class ColumnDescriptionUniquiIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Species",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Specialties",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "DocumentTypes",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_Species_Description",
                table: "Species",
                column: "Description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Specialties_Description",
                table: "Specialties",
                column: "Description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTypes_Description",
                table: "DocumentTypes",
                column: "Description",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Species_Description",
                table: "Species");

            migrationBuilder.DropIndex(
                name: "IX_Specialties_Description",
                table: "Specialties");

            migrationBuilder.DropIndex(
                name: "IX_DocumentTypes_Description",
                table: "DocumentTypes");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Species",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Specialties",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "DocumentTypes",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}

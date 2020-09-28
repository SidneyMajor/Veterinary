using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Veterinary.Migrations
{
    public partial class AddSpecialty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animal_Species_SpeciesID",
                table: "Animal");

            migrationBuilder.DropForeignKey(
                name: "FK_Animal_AspNetUsers_UserId",
                table: "Animal");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Animal",
                table: "Animal");

            migrationBuilder.RenameTable(
                name: "Animal",
                newName: "Animals");

            migrationBuilder.RenameIndex(
                name: "IX_Animal_UserId",
                table: "Animals",
                newName: "IX_Animals_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Animal_SpeciesID",
                table: "Animals",
                newName: "IX_Animals_SpeciesID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Animals",
                table: "Animals",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Specialties",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    WasDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialties", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Animals_Species_SpeciesID",
                table: "Animals",
                column: "SpeciesID",
                principalTable: "Species",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Animals_AspNetUsers_UserId",
                table: "Animals",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animals_Species_SpeciesID",
                table: "Animals");

            migrationBuilder.DropForeignKey(
                name: "FK_Animals_AspNetUsers_UserId",
                table: "Animals");

            migrationBuilder.DropTable(
                name: "Specialties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Animals",
                table: "Animals");

            migrationBuilder.RenameTable(
                name: "Animals",
                newName: "Animal");

            migrationBuilder.RenameIndex(
                name: "IX_Animals_UserId",
                table: "Animal",
                newName: "IX_Animal_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Animals_SpeciesID",
                table: "Animal",
                newName: "IX_Animal_SpeciesID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Animal",
                table: "Animal",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Animal_Species_SpeciesID",
                table: "Animal",
                column: "SpeciesID",
                principalTable: "Species",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Animal_AspNetUsers_UserId",
                table: "Animal",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

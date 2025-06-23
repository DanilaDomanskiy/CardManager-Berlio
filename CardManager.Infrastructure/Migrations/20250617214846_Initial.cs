using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CardRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Track1 = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Track2 = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Track3 = table.Column<string>(type: "nvarchar(110)", maxLength: 110, nullable: false),
                    Created = table.Column<DateOnly>(type: "date", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardRecords_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardRecords_CardNumber",
                table: "CardRecords",
                column: "CardNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CardRecords_CreatorId",
                table: "CardRecords",
                column: "CreatorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardRecords");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

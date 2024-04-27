using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DummyAuthorizationProvider.Data.Db.Migrations
{
    /// <inheritdoc />
    public partial class CreateVoterTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Voters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Oib = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voters", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Voters_Oib",
                table: "Voters",
                column: "Oib",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Voters");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VotingApp.Data.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddPeerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Peers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Port = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Peers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Peers_IpAddress_Port",
                table: "Peers",
                columns: new[] { "IpAddress", "Port" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Peers");
        }
    }
}

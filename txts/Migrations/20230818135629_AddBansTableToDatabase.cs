using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using txts.Database;

#nullable disable

namespace txts.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20230818135629_AddBansTableToDatabase")]
    public partial class AddBansTableToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bans",
                columns: table => new
                {
                    BanId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PageId = table.Column<int>(type: "INTEGER", nullable: false),
                    Reason = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bans", x => x.BanId);
                    table.ForeignKey(
                        name: "FK_Bans_Pages_PageId",
                        column: x => x.PageId,
                        principalTable: "Pages",
                        principalColumn: "PageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bans_PageId",
                table: "Bans",
                column: "PageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bans");
        }
    }
}

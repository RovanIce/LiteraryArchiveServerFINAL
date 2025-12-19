using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pleaseworkplease.Migrations
{
    /// <inheritdoc />
    public partial class help : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Novels",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ISBN = table.Column<int>(nullable: false),
                    Genre = table.Column<int>(nullable: false),
                    Title = table.Column<string>(type: "varchar(50)", nullable: false),
                    Author = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Novels", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Novels_Genre",
                        column: x => x.Genre,
                        principalTable: "Genre",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Novels_Genre",
                table: "Novels",
                column: "Genre");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Novels");
        }
    }
}

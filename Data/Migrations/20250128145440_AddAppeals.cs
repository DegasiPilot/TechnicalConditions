using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechnicalConditions.Migrations
{
    /// <inheritdoc />
    public partial class AddAppeals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppealCases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppealCases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppealerCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppealerCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppealPurposes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppealPurposes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Appeals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppealerCategoryId = table.Column<int>(type: "int", nullable: false),
                    AppealPurposeId = table.Column<int>(type: "int", nullable: false),
                    AppealCaseId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appeals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appeals_AppealCases_AppealCaseId",
                        column: x => x.AppealCaseId,
                        principalTable: "AppealCases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appeals_AppealPurposes_AppealPurposeId",
                        column: x => x.AppealPurposeId,
                        principalTable: "AppealPurposes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appeals_AppealerCategories_AppealerCategoryId",
                        column: x => x.AppealerCategoryId,
                        principalTable: "AppealerCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appeals_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentBytes = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    AppealId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_Appeals_AppealId",
                        column: x => x.AppealId,
                        principalTable: "Appeals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appeals_AppealCaseId",
                table: "Appeals",
                column: "AppealCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Appeals_AppealerCategoryId",
                table: "Appeals",
                column: "AppealerCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Appeals_AppealPurposeId",
                table: "Appeals",
                column: "AppealPurposeId");

            migrationBuilder.CreateIndex(
                name: "IX_Appeals_UserId",
                table: "Appeals",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_AppealId",
                table: "Documents",
                column: "AppealId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Appeals");

            migrationBuilder.DropTable(
                name: "AppealCases");

            migrationBuilder.DropTable(
                name: "AppealPurposes");

            migrationBuilder.DropTable(
                name: "AppealerCategories");
        }
    }
}

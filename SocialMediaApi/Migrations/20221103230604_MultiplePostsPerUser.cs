using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMediaApi.Migrations
{
    public partial class MultiplePostsPerUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Posts_PostId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_PostId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "PostUser",
                columns: table => new
                {
                    LikedById = table.Column<int>(type: "INTEGER", nullable: false),
                    LikedPostsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostUser", x => new { x.LikedById, x.LikedPostsId });
                    table.ForeignKey(
                        name: "FK_PostUser_Posts_LikedPostsId",
                        column: x => x.LikedPostsId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostUser_Users_LikedById",
                        column: x => x.LikedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostUser_LikedPostsId",
                table: "PostUser",
                column: "LikedPostsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostUser");

            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "Users",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_PostId",
                table: "Users",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Posts_PostId",
                table: "Users",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");
        }
    }
}

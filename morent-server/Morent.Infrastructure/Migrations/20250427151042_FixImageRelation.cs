using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Morent.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixImageRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_ProfileImageId",
                table: "Users",
                column: "ProfileImageId");

            migrationBuilder.CreateIndex(
                name: "IX_MorentCarImage_ImageId",
                table: "MorentCarImage",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_MorentCarImage_MorentImage_ImageId",
                table: "MorentCarImage",
                column: "ImageId",
                principalTable: "MorentImage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_MorentImage_ProfileImageId",
                table: "Users",
                column: "ProfileImageId",
                principalTable: "MorentImage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MorentCarImage_MorentImage_ImageId",
                table: "MorentCarImage");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_MorentImage_ProfileImageId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ProfileImageId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_MorentCarImage_ImageId",
                table: "MorentCarImage");
        }
    }
}

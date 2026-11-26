using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EsnafPlatformu.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddBusinessOwner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerUserId",
                table: "Businesses",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ux_businesses_owner_user_id",
                table: "Businesses",
                column: "OwnerUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Businesses_AspNetUsers_OwnerUserId",
                table: "Businesses",
                column: "OwnerUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Businesses_AspNetUsers_OwnerUserId",
                table: "Businesses");

            migrationBuilder.DropIndex(
                name: "ux_businesses_owner_user_id",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "OwnerUserId",
                table: "Businesses");
        }
    }
}

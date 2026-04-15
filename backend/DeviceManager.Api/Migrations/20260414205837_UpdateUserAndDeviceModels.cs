using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeviceManager.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserAndDeviceModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssignedUserId",
                table: "Devices",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Devices_AssignedUserId",
                table: "Devices",
                column: "AssignedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_AspNetUsers_AssignedUserId",
                table: "Devices",
                column: "AssignedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_AspNetUsers_AssignedUserId",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Devices_AssignedUserId",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "AssignedUserId",
                table: "Devices");
        }
    }
}

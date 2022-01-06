using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Migrations
{
    public partial class addUserName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_ChatUser<string>_UserId",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatUser<string>",
                table: "ChatUser<string>");

            migrationBuilder.RenameTable(
                name: "ChatUser<string>",
                newName: "Users");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_UserId",
                table: "Messages",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_UserId",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "ChatUser<string>");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatUser<string>",
                table: "ChatUser<string>",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_ChatUser<string>_UserId",
                table: "Messages",
                column: "UserId",
                principalTable: "ChatUser<string>",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

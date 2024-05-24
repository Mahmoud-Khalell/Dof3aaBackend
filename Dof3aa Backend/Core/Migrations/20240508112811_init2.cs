using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AspNetUsers_PublisherUserUserName",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_PublisherUserUserName",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "PublisherUserUserName",
                table: "Tasks");

            migrationBuilder.AlterColumn<string>(
                name: "PublisherUserName",
                table: "Tasks",
                type: "nvarchar(256)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Announcements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourceId = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PublisherUserName = table.Column<string>(type: "nvarchar(256)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Announcements_AspNetUsers_PublisherUserName",
                        column: x => x.PublisherUserName,
                        principalTable: "AspNetUsers",
                        principalColumn: "UserName",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Announcements_Cources_CourceId",
                        column: x => x.CourceId,
                        principalTable: "Cources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    publiserUsername = table.Column<string>(type: "nvarchar(256)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_publiserUsername",
                        column: x => x.publiserUsername,
                        principalTable: "AspNetUsers",
                        principalColumn: "UserName",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "UserNotifications",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    ReceiverUserName = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNotifications", x => new { x.NotificationId, x.ReceiverUserName });
                    table.ForeignKey(
                        name: "FK_UserNotifications_AspNetUsers_ReceiverUserName",
                        column: x => x.ReceiverUserName,
                        principalTable: "AspNetUsers",
                        principalColumn: "UserName",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_UserNotifications_Notifications_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_PublisherUserName",
                table: "Tasks",
                column: "PublisherUserName");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_CourceId",
                table: "Announcements",
                column: "CourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_PublisherUserName",
                table: "Announcements",
                column: "PublisherUserName");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_publiserUsername",
                table: "Notifications",
                column: "publiserUsername");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_ReceiverUserName",
                table: "UserNotifications",
                column: "ReceiverUserName");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AspNetUsers_PublisherUserName",
                table: "Tasks",
                column: "PublisherUserName",
                principalTable: "AspNetUsers",
                principalColumn: "UserName",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AspNetUsers_PublisherUserName",
                table: "Tasks");

            migrationBuilder.DropTable(
                name: "Announcements");

            migrationBuilder.DropTable(
                name: "UserNotifications");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_PublisherUserName",
                table: "Tasks");

            migrationBuilder.AlterColumn<string>(
                name: "PublisherUserName",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)");

            migrationBuilder.AddColumn<string>(
                name: "PublisherUserUserName",
                table: "Tasks",
                type: "nvarchar(256)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_PublisherUserUserName",
                table: "Tasks",
                column: "PublisherUserUserName");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AspNetUsers_PublisherUserUserName",
                table: "Tasks",
                column: "PublisherUserUserName",
                principalTable: "AspNetUsers",
                principalColumn: "UserName");
        }
    }
}

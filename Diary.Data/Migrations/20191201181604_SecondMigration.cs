using Microsoft.EntityFrameworkCore.Migrations;

namespace Diary.Data.Migrations
{
    public partial class SecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NotificationEventId",
                table: "Events",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NotificationJobId",
                table: "Assignments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NotificationEventId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "NotificationJobId",
                table: "Assignments");
        }
    }
}

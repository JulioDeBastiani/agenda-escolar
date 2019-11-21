using Microsoft.EntityFrameworkCore.Migrations;

namespace Diary.Data.Migrations
{
    public partial class SecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserGuardians_StudentId",
                table: "UserGuardians");

            migrationBuilder.DropIndex(
                name: "IX_UserEvents_UserId",
                table: "UserEvents");

            migrationBuilder.DropIndex(
                name: "IX_StudentClasses_StudentId",
                table: "StudentClasses");

            migrationBuilder.DropIndex(
                name: "IX_Grades_AssignmentId",
                table: "Grades");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserGuardians_StudentId_GuardianId",
                table: "UserGuardians",
                columns: new[] { "StudentId", "GuardianId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserEvents_UserId_EventId",
                table: "UserEvents",
                columns: new[] { "UserId", "EventId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentClasses_StudentId_ClassId",
                table: "StudentClasses",
                columns: new[] { "StudentId", "ClassId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SchoolYears_Year",
                table: "SchoolYears",
                column: "Year",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Grades_AssignmentId_StudentId",
                table: "Grades",
                columns: new[] { "AssignmentId", "StudentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_Date_StudentClassId",
                table: "Attendances",
                columns: new[] { "Date", "StudentClassId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Username",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_UserGuardians_StudentId_GuardianId",
                table: "UserGuardians");

            migrationBuilder.DropIndex(
                name: "IX_UserEvents_UserId_EventId",
                table: "UserEvents");

            migrationBuilder.DropIndex(
                name: "IX_StudentClasses_StudentId_ClassId",
                table: "StudentClasses");

            migrationBuilder.DropIndex(
                name: "IX_SchoolYears_Year",
                table: "SchoolYears");

            migrationBuilder.DropIndex(
                name: "IX_Grades_AssignmentId_StudentId",
                table: "Grades");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_Date_StudentClassId",
                table: "Attendances");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserGuardians_StudentId",
                table: "UserGuardians",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserEvents_UserId",
                table: "UserEvents",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentClasses_StudentId",
                table: "StudentClasses",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_AssignmentId",
                table: "Grades",
                column: "AssignmentId");
        }
    }
}

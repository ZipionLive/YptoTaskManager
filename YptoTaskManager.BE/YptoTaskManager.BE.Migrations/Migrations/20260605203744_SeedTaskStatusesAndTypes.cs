using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YptoTaskManager.BE.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class SeedTaskStatusesAndTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var systemUserId = new Guid("11111111-1111-1111-1111-111111111111");

            var systemSalt = new Guid("22222222-2222-2222-2222-222222222222");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[]
                {
                    "Id",
                    "FirstName",
                    "LastName",
                    "Email",
                    "PhoneNumber",
                    "PasswordSalt",
                    "PasswordHash"
                },
                values: new object[]
                {
                    systemUserId,
                    "System",
                    "Administrator",
                    "system@yptotaskmanager.local",
                    "0000000000",
                    systemSalt,
                    "SEED_ONLY"
                });

            migrationBuilder.InsertData(
                table: "TaskItemStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "To do" },
                    { 2, "In progress" },
                    { 3, "Done" },
                    { 4, "Deleted" }
                });

            migrationBuilder.InsertData(
                table: "TaskItemTypes",
                columns: new[] { "Id", "CreatedById", "Name", "Description" },
                values: new object[,]
                {
                    { 1, systemUserId, "Work", "Professional activities" },
                    { 2, systemUserId, "Personal", "Personal activities" },
                    { 3, systemUserId, "Leisure", "Leisure activities" }
                });

            migrationBuilder.InsertData(
                table: "TaskItemTypes",
                columns: new[] { "Id", "CreatedById", "Name", "Description", "ParentId" },
                values: new object[,]
                {
                    { 4, systemUserId, "Development", "Software development tasks", 1 },
                    { 5, systemUserId, "Testing", "Software testing tasks", 1 },
                    { 6, systemUserId, "Documentation", "Documentation redaction tasks", 1 },
                    { 7, systemUserId, "Shopping", "Shopping related tasks", 2 },
                    { 8, systemUserId, "Travel", "Travel related tasks", 2 },
                    { 9, systemUserId, "Health", "Health related tasks", 2 },
                    { 10, systemUserId, "Music", "Music related tasks", 3 },
                    { 11, systemUserId, "Sport", "Sport related tasks", 3 },
                    { 12, systemUserId, "Gaming", "Gaming related tasks", 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TaskItemTypes",
                keyColumn: "Id",
                keyValues: new object[]
                {
            4, 5, 6, 7, 8, 9, 10, 11, 12
                });

            migrationBuilder.DeleteData(
                table: "TaskItemTypes",
                keyColumn: "Id",
                keyValues: new object[]
                {
            1, 2, 3
                });

            migrationBuilder.DeleteData(
                table: "TaskItemStatuses",
                keyColumn: "Id",
                keyValues: new object[]
                {
            1, 2, 3, 4
                });

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));
        }
    }
}

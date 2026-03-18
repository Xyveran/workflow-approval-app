using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WorkflowApproval.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class newSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowCondition_WorkflowSteps_WorkflowStepId",
                table: "WorkflowCondition");

            migrationBuilder.DropTable(
                name: "WorkflowSteps");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "ApprovalActions");

            migrationBuilder.DropColumn(
                name: "WorkflowStepId",
                table: "ApprovalActions");

            migrationBuilder.RenameColumn(
                name: "isActive",
                table: "WorkflowDefinitions",
                newName: "IsActive");

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "WorkflowDefinitions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "WorkflowCondition",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Operator",
                table: "WorkflowCondition",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "FieldName",
                table: "WorkflowCondition",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Department",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "RequestTypes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.Sql(
                @"ALTER TABLE ""Requests"" ALTER COLUMN ""Status"" TYPE integer USING ""Status""::integer");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "Requests",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "Requests",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Requests",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Requests",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "WorkflowDefinitionId",
                table: "Requests",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "ActionDate",
                table: "ApprovalActions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "ApprovalActions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StepOrder",
                table: "ApprovalActions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "WorkflowInstances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RequestId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkflowDefinitionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowInstances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowStepDefinitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkflowDefinitionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    StepOrder = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    StepType = table.Column<int>(type: "integer", nullable: false),
                    AllowParallelApprovals = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowStepDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowStepDefinitions_WorkflowDefinitions_WorkflowDefinit~",
                        column: x => x.WorkflowDefinitionId,
                        principalTable: "WorkflowDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowStepInstances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkflowInstanceId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkflowStepId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    StepOrder = table.Column<int>(type: "integer", nullable: false),
                    RoleRequired = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowStepInstances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowStepInstances_WorkflowInstances_WorkflowInstanceId",
                        column: x => x.WorkflowInstanceId,
                        principalTable: "WorkflowInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkflowStepId = table.Column<Guid>(type: "uuid", nullable: false),
                    Field = table.Column<string>(type: "text", nullable: false),
                    Operator = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowRules_WorkflowStepDefinitions_WorkflowStepId",
                        column: x => x.WorkflowStepId,
                        principalTable: "WorkflowStepDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                column: "Email",
                value: null);

            migrationBuilder.UpdateData(
                table: "WorkflowDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                columns: new[] { "IsActive", "Version" },
                values: new object[] { true, 1 });

            migrationBuilder.InsertData(
                table: "WorkflowStepDefinitions",
                columns: new[] { "Id", "AllowParallelApprovals", "Name", "RoleId", "StepOrder", "StepType", "WorkflowDefinitionId" },
                values: new object[,]
                {
                    { new Guid("99999999-9999-9999-9999-999999999998"), false, "Admin Sign-off", new Guid("11111111-1111-1111-1111-111111111111"), 3, 0, new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd") },
                    { new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), false, "Manager Approval", new Guid("22222222-2222-2222-2222-222222222222"), 1, 0, new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd") },
                    { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), false, "Finance Approval", new Guid("33333333-3333-3333-3333-333333333333"), 2, 0, new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Requests_WorkflowDefinitionId",
                table: "Requests",
                column: "WorkflowDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalActions_ActionDate",
                table: "ApprovalActions",
                column: "ActionDate");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalActions_CreatedAt",
                table: "ApprovalActions",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalActions_RequestId",
                table: "ApprovalActions",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalActions_StepOrder",
                table: "ApprovalActions",
                column: "StepOrder");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowRules_WorkflowStepId",
                table: "WorkflowRules",
                column: "WorkflowStepId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowStepDefinitions_WorkflowDefinitionId",
                table: "WorkflowStepDefinitions",
                column: "WorkflowDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowStepInstances_WorkflowInstanceId",
                table: "WorkflowStepInstances",
                column: "WorkflowInstanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalActions_Requests_RequestId",
                table: "ApprovalActions",
                column: "RequestId",
                principalTable: "Requests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_WorkflowDefinitions_WorkflowDefinitionId",
                table: "Requests",
                column: "WorkflowDefinitionId",
                principalTable: "WorkflowDefinitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowCondition_WorkflowStepDefinitions_WorkflowStepId",
                table: "WorkflowCondition",
                column: "WorkflowStepId",
                principalTable: "WorkflowStepDefinitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalActions_Requests_RequestId",
                table: "ApprovalActions");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_WorkflowDefinitions_WorkflowDefinitionId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowCondition_WorkflowStepDefinitions_WorkflowStepId",
                table: "WorkflowCondition");

            migrationBuilder.DropTable(
                name: "WorkflowRules");

            migrationBuilder.DropTable(
                name: "WorkflowStepInstances");

            migrationBuilder.DropTable(
                name: "WorkflowStepDefinitions");

            migrationBuilder.DropTable(
                name: "WorkflowInstances");

            migrationBuilder.DropIndex(
                name: "IX_Requests_WorkflowDefinitionId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_ApprovalActions_ActionDate",
                table: "ApprovalActions");

            migrationBuilder.DropIndex(
                name: "IX_ApprovalActions_CreatedAt",
                table: "ApprovalActions");

            migrationBuilder.DropIndex(
                name: "IX_ApprovalActions_RequestId",
                table: "ApprovalActions");

            migrationBuilder.DropIndex(
                name: "IX_ApprovalActions_StepOrder",
                table: "ApprovalActions");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "WorkflowDefinitions");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "WorkflowDefinitionId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ActionDate",
                table: "ApprovalActions");

            migrationBuilder.DropColumn(
                name: "Comments",
                table: "ApprovalActions");

            migrationBuilder.DropColumn(
                name: "StepOrder",
                table: "ApprovalActions");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "WorkflowDefinitions",
                newName: "isActive");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "WorkflowCondition",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Operator",
                table: "WorkflowCondition",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FieldName",
                table: "WorkflowCondition",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Department",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "RequestTypes",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.Sql(
                @"ALTER TABLE ""Requests"" ALTER COLUMN ""Status"" TYPE text USING ""Status""::text");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "ApprovalActions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "WorkflowStepId",
                table: "ApprovalActions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "WorkflowSteps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkflowDefinitionId = table.Column<Guid>(type: "uuid", nullable: false),
                    AllowParallelApprovals = table.Column<bool>(type: "boolean", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    StepOrder = table.Column<int>(type: "integer", nullable: false),
                    StepType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowSteps_WorkflowDefinitions_WorkflowDefinitionId",
                        column: x => x.WorkflowDefinitionId,
                        principalTable: "WorkflowDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                column: "Email",
                value: "");

            migrationBuilder.UpdateData(
                table: "WorkflowDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                column: "isActive",
                value: false);

            migrationBuilder.InsertData(
                table: "WorkflowSteps",
                columns: new[] { "Id", "AllowParallelApprovals", "RoleId", "StepOrder", "StepType", "WorkflowDefinitionId" },
                values: new object[,]
                {
                    { new Guid("99999999-9999-9999-9999-999999999999"), false, new Guid("11111111-1111-1111-1111-111111111111"), 3, 0, new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd") },
                    { new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), false, new Guid("22222222-2222-2222-2222-222222222222"), 1, 0, new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd") },
                    { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), false, new Guid("33333333-3333-3333-3333-333333333333"), 2, 0, new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowSteps_WorkflowDefinitionId",
                table: "WorkflowSteps",
                column: "WorkflowDefinitionId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowCondition_WorkflowSteps_WorkflowStepId",
                table: "WorkflowCondition",
                column: "WorkflowStepId",
                principalTable: "WorkflowSteps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntapTest.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeActivityName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeActivities_Activities_ActivityId",
                table: "TimeActivities");

            migrationBuilder.DropColumn(
                name: "AcitvityId",
                table: "TimeActivities");

            migrationBuilder.AlterColumn<Guid>(
                name: "ActivityId",
                table: "TimeActivities",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeActivities_Activities_ActivityId",
                table: "TimeActivities",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeActivities_Activities_ActivityId",
                table: "TimeActivities");

            migrationBuilder.AlterColumn<Guid>(
                name: "ActivityId",
                table: "TimeActivities",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "AcitvityId",
                table: "TimeActivities",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_TimeActivities_Activities_ActivityId",
                table: "TimeActivities",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id");
        }
    }
}

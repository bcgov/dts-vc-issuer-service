using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Issuer.Migrations
{
    public partial class AddIdentifier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImmunizationRecordUrl",
                table: "Credential");

            migrationBuilder.AddColumn<int>(
                name: "IdentifierId",
                table: "Credential",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Identifier",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedUserId = table.Column<Guid>(nullable: false),
                    CreatedTimeStamp = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedUserId = table.Column<Guid>(nullable: false),
                    UpdatedTimeStamp = table.Column<DateTimeOffset>(nullable: false),
                    Guid = table.Column<Guid>(nullable: false),
                    Uri = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identifier", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Credential_IdentifierId",
                table: "Credential",
                column: "IdentifierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Credential_Identifier_IdentifierId",
                table: "Credential",
                column: "IdentifierId",
                principalTable: "Identifier",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Credential_Identifier_IdentifierId",
                table: "Credential");

            migrationBuilder.DropTable(
                name: "Identifier");

            migrationBuilder.DropIndex(
                name: "IX_Credential_IdentifierId",
                table: "Credential");

            migrationBuilder.DropColumn(
                name: "IdentifierId",
                table: "Credential");

            migrationBuilder.AddColumn<string>(
                name: "ImmunizationRecordUrl",
                table: "Credential",
                type: "text",
                nullable: true);
        }
    }
}

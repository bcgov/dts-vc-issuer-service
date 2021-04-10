using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Issuer.Migrations
{
    public partial class AddConnection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientCredential");

            migrationBuilder.DropColumn(
                name: "Alias",
                table: "Credential");

            migrationBuilder.DropColumn(
                name: "Base64QRCode",
                table: "Credential");

            migrationBuilder.DropColumn(
                name: "ConnectionId",
                table: "Credential");

            migrationBuilder.AddColumn<int>(
                name: "ConnectionId",
                table: "Credential",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "ImmunizationRecordUrl",
                table: "Credential",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Connection",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedUserId = table.Column<Guid>(nullable: false),
                    CreatedTimeStamp = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedUserId = table.Column<Guid>(nullable: false),
                    UpdatedTimeStamp = table.Column<DateTimeOffset>(nullable: false),
                    PatientId = table.Column<int>(nullable: false),
                    ConnectionId = table.Column<string>(nullable: true),
                    Base64QRCode = table.Column<string>(nullable: true),
                    AcceptedConnectionDate = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Connection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Connection_Patient_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Credential_ConnectionId",
                table: "Credential",
                column: "ConnectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Connection_PatientId",
                table: "Connection",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Credential_Connection_ConnectionId",
                table: "Credential",
                column: "ConnectionId",
                principalTable: "Connection",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Credential_Connection_ConnectionId",
                table: "Credential");

            migrationBuilder.DropTable(
                name: "Connection");

            migrationBuilder.DropIndex(
                name: "IX_Credential_ConnectionId",
                table: "Credential");

            migrationBuilder.DropColumn(
                name: "ImmunizationRecordUrl",
                table: "Credential");

            migrationBuilder.AlterColumn<string>(
                name: "ConnectionId",
                table: "Credential",
                type: "text",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "Credential",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Base64QRCode",
                table: "Credential",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PatientCredential",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedTimeStamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CredentialId = table.Column<int>(type: "integer", nullable: false),
                    PatientId = table.Column<int>(type: "integer", nullable: false),
                    UpdatedTimeStamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedUserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientCredential", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientCredential_Credential_CredentialId",
                        column: x => x.CredentialId,
                        principalTable: "Credential",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientCredential_Patient_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientCredential_CredentialId",
                table: "PatientCredential",
                column: "CredentialId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientCredential_PatientId",
                table: "PatientCredential",
                column: "PatientId");
        }
    }
}

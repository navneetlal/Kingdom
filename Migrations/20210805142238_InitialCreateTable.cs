using System;
using System.Text.Json;
using KingdomApi.Models;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KingdomApi.Migrations
{
    public partial class InitialCreateTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "kingdoms",
                columns: table => new
                {
                    kingdom_id = table.Column<Guid>(type: "uuid", nullable: false),
                    kingdom_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_kingdoms", x => x.kingdom_id);
                });

            migrationBuilder.CreateTable(
                name: "clans",
                columns: table => new
                {
                    clan_id = table.Column<Guid>(type: "uuid", nullable: false),
                    clan_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    clan_purpose = table.Column<string>(type: "text", nullable: true),
                    kingdom_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_clans", x => x.clan_id);
                    table.ForeignKey(
                        name: "fk_clans_kingdoms_kingdom_id",
                        column: x => x.kingdom_id,
                        principalTable: "kingdoms",
                        principalColumn: "kingdom_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "nobles",
                columns: table => new
                {
                    noble_id = table.Column<Guid>(type: "uuid", nullable: false),
                    username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    full_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email_address = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    date_of_birth = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    gender = table.Column<string>(type: "varchar(24)", nullable: false),
                    organization_name = table.Column<string>(type: "text", nullable: true),
                    department = table.Column<string>(type: "text", nullable: true),
                    job_title = table.Column<string>(type: "text", nullable: true),
                    employee_id = table.Column<string>(type: "text", nullable: true),
                    reporting_manager = table.Column<string>(type: "text", nullable: true),
                    address = table.Column<string>(type: "text", nullable: true),
                    city = table.Column<string>(type: "text", nullable: true),
                    state = table.Column<string>(type: "text", nullable: true),
                    country = table.Column<string>(type: "text", nullable: true),
                    postal_code = table.Column<int>(type: "integer", nullable: false),
                    kingdom_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_nobles", x => x.noble_id);
                    table.ForeignKey(
                        name: "fk_nobles_kingdoms_kingdom_id",
                        column: x => x.kingdom_id,
                        principalTable: "kingdoms",
                        principalColumn: "kingdom_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "responsibilities",
                columns: table => new
                {
                    responsibility_id = table.Column<Guid>(type: "uuid", nullable: false),
                    responsibility_name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    target = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    action = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                    depth = table.Column<string>(type: "varchar(24)", nullable: false),
                    effect = table.Column<string>(type: "varchar(24)", nullable: false),
                    condition = table.Column<JsonDocument>(type: "jsonb", nullable: true),
                    obligation = table.Column<Obligation>(type: "jsonb", nullable: true),
                    priority = table.Column<short>(type: "smallint", nullable: false),
                    kingdom_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_responsibilities", x => x.responsibility_id);
                    table.ForeignKey(
                        name: "fk_responsibilities_kingdoms_kingdom_id",
                        column: x => x.kingdom_id,
                        principalTable: "kingdoms",
                        principalColumn: "kingdom_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "clan_noble",
                columns: table => new
                {
                    clans_clan_id = table.Column<Guid>(type: "uuid", nullable: false),
                    nobles_noble_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_clan_noble", x => new { x.clans_clan_id, x.nobles_noble_id });
                    table.ForeignKey(
                        name: "fk_clan_noble_clans_clans_clan_id",
                        column: x => x.clans_clan_id,
                        principalTable: "clans",
                        principalColumn: "clan_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_clan_noble_nobles_nobles_noble_id",
                        column: x => x.nobles_noble_id,
                        principalTable: "nobles",
                        principalColumn: "noble_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "noble_secrets",
                columns: table => new
                {
                    username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email_address = table.Column<string>(type: "text", nullable: true),
                    password = table.Column<string>(type: "text", nullable: true),
                    noble_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_noble_secrets", x => x.username);
                    table.ForeignKey(
                        name: "fk_noble_secrets_nobles_noble_id",
                        column: x => x.noble_id,
                        principalTable: "nobles",
                        principalColumn: "noble_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "clan_responsibility",
                columns: table => new
                {
                    clans_clan_id = table.Column<Guid>(type: "uuid", nullable: false),
                    responsibilities_responsibility_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_clan_responsibility", x => new { x.clans_clan_id, x.responsibilities_responsibility_id });
                    table.ForeignKey(
                        name: "fk_clan_responsibility_clans_clans_clan_id",
                        column: x => x.clans_clan_id,
                        principalTable: "clans",
                        principalColumn: "clan_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_clan_responsibility_responsibilities_responsibilities_respo",
                        column: x => x.responsibilities_responsibility_id,
                        principalTable: "responsibilities",
                        principalColumn: "responsibility_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "noble_responsibility",
                columns: table => new
                {
                    nobles_noble_id = table.Column<Guid>(type: "uuid", nullable: false),
                    responsibilities_responsibility_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_noble_responsibility", x => new { x.nobles_noble_id, x.responsibilities_responsibility_id });
                    table.ForeignKey(
                        name: "fk_noble_responsibility_nobles_nobles_noble_id",
                        column: x => x.nobles_noble_id,
                        principalTable: "nobles",
                        principalColumn: "noble_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_noble_responsibility_responsibilities_responsibilities_resp",
                        column: x => x.responsibilities_responsibility_id,
                        principalTable: "responsibilities",
                        principalColumn: "responsibility_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_clan_noble_nobles_noble_id",
                table: "clan_noble",
                column: "nobles_noble_id");

            migrationBuilder.CreateIndex(
                name: "ix_clan_responsibility_responsibilities_responsibility_id",
                table: "clan_responsibility",
                column: "responsibilities_responsibility_id");

            migrationBuilder.CreateIndex(
                name: "ix_clans_kingdom_id",
                table: "clans",
                column: "kingdom_id");

            migrationBuilder.CreateIndex(
                name: "ix_noble_responsibility_responsibilities_responsibility_id",
                table: "noble_responsibility",
                column: "responsibilities_responsibility_id");

            migrationBuilder.CreateIndex(
                name: "ix_noble_secrets_email_address",
                table: "noble_secrets",
                column: "email_address",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_noble_secrets_noble_id",
                table: "noble_secrets",
                column: "noble_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_noble_secrets_username",
                table: "noble_secrets",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_nobles_kingdom_id",
                table: "nobles",
                column: "kingdom_id");

            migrationBuilder.CreateIndex(
                name: "ix_responsibilities_kingdom_id",
                table: "responsibilities",
                column: "kingdom_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "clan_noble");

            migrationBuilder.DropTable(
                name: "clan_responsibility");

            migrationBuilder.DropTable(
                name: "noble_responsibility");

            migrationBuilder.DropTable(
                name: "noble_secrets");

            migrationBuilder.DropTable(
                name: "clans");

            migrationBuilder.DropTable(
                name: "responsibilities");

            migrationBuilder.DropTable(
                name: "nobles");

            migrationBuilder.DropTable(
                name: "kingdoms");
        }
    }
}

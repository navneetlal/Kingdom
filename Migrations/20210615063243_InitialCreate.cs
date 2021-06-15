using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KingdomApi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "kingdoms",
                columns: table => new
                {
                    kingdom_id = table.Column<long>(type: "bigint", nullable: false),
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
                    clan_id = table.Column<long>(type: "bigint", nullable: false),
                    clan_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    clan_purpose = table.Column<string>(type: "text", nullable: true),
                    kingdom_id = table.Column<long>(type: "bigint", nullable: false)
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
                name: "noblemen",
                columns: table => new
                {
                    nobleman_id = table.Column<long>(type: "bigint", nullable: false),
                    username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    password = table.Column<string>(type: "text", nullable: true),
                    full_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email_address = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    date_of_birth = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    gender = table.Column<int>(type: "integer", nullable: false),
                    organization_name = table.Column<string>(type: "text", nullable: true),
                    department = table.Column<string>(type: "text", nullable: true),
                    job_title = table.Column<string>(type: "text", nullable: true),
                    employee_id = table.Column<string>(type: "text", nullable: true),
                    reporting_manager = table.Column<string>(type: "text", nullable: true),
                    address = table.Column<string>(type: "text", nullable: true),
                    city = table.Column<string>(type: "text", nullable: true),
                    state = table.Column<string>(type: "text", nullable: true),
                    country = table.Column<string>(type: "text", nullable: true),
                    postal_code = table.Column<long>(type: "bigint", nullable: false),
                    kingdom_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_noblemen", x => x.nobleman_id);
                    table.ForeignKey(
                        name: "fk_noblemen_kingdoms_kingdom_id",
                        column: x => x.kingdom_id,
                        principalTable: "kingdoms",
                        principalColumn: "kingdom_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "responsibilities",
                columns: table => new
                {
                    responsibility_id = table.Column<long>(type: "bigint", nullable: false),
                    responsibility_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    resource_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    action = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    action_level = table.Column<int>(type: "integer", nullable: false),
                    kingdom_id = table.Column<long>(type: "bigint", nullable: false)
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
                name: "clan_nobleman",
                columns: table => new
                {
                    clans_clan_id = table.Column<long>(type: "bigint", nullable: false),
                    noblemen_nobleman_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_clan_nobleman", x => new { x.clans_clan_id, x.noblemen_nobleman_id });
                    table.ForeignKey(
                        name: "fk_clan_nobleman_clans_clans_clan_id",
                        column: x => x.clans_clan_id,
                        principalTable: "clans",
                        principalColumn: "clan_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_clan_nobleman_noblemen_noblemen_nobleman_id",
                        column: x => x.noblemen_nobleman_id,
                        principalTable: "noblemen",
                        principalColumn: "nobleman_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "clan_responsibility",
                columns: table => new
                {
                    clans_clan_id = table.Column<long>(type: "bigint", nullable: false),
                    responsibilities_responsibility_id = table.Column<long>(type: "bigint", nullable: false)
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
                name: "nobleman_responsibility",
                columns: table => new
                {
                    noblemen_nobleman_id = table.Column<long>(type: "bigint", nullable: false),
                    responsibilities_responsibility_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_nobleman_responsibility", x => new { x.noblemen_nobleman_id, x.responsibilities_responsibility_id });
                    table.ForeignKey(
                        name: "fk_nobleman_responsibility_noblemen_noblemen_nobleman_id",
                        column: x => x.noblemen_nobleman_id,
                        principalTable: "noblemen",
                        principalColumn: "nobleman_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_nobleman_responsibility_responsibilities_responsibilities_r",
                        column: x => x.responsibilities_responsibility_id,
                        principalTable: "responsibilities",
                        principalColumn: "responsibility_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_clan_nobleman_noblemen_nobleman_id",
                table: "clan_nobleman",
                column: "noblemen_nobleman_id");

            migrationBuilder.CreateIndex(
                name: "ix_clan_responsibility_responsibilities_responsibility_id",
                table: "clan_responsibility",
                column: "responsibilities_responsibility_id");

            migrationBuilder.CreateIndex(
                name: "ix_clans_kingdom_id",
                table: "clans",
                column: "kingdom_id");

            migrationBuilder.CreateIndex(
                name: "ix_nobleman_responsibility_responsibilities_responsibility_id",
                table: "nobleman_responsibility",
                column: "responsibilities_responsibility_id");

            migrationBuilder.CreateIndex(
                name: "ix_noblemen_kingdom_id",
                table: "noblemen",
                column: "kingdom_id");

            migrationBuilder.CreateIndex(
                name: "ix_responsibilities_kingdom_id",
                table: "responsibilities",
                column: "kingdom_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "clan_nobleman");

            migrationBuilder.DropTable(
                name: "clan_responsibility");

            migrationBuilder.DropTable(
                name: "nobleman_responsibility");

            migrationBuilder.DropTable(
                name: "clans");

            migrationBuilder.DropTable(
                name: "noblemen");

            migrationBuilder.DropTable(
                name: "responsibilities");

            migrationBuilder.DropTable(
                name: "kingdoms");
        }
    }
}

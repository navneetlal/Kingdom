﻿// <auto-generated />
using System;
using System.Text.Json;
using KingdomApi;
using KingdomApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace KingdomApi.Migrations
{
    [DbContext(typeof(KingdomContext))]
    partial class KingdomContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("ClanNoble", b =>
                {
                    b.Property<Guid>("ClansClanId")
                        .HasColumnType("uuid")
                        .HasColumnName("clans_clan_id");

                    b.Property<Guid>("NoblesNobleId")
                        .HasColumnType("uuid")
                        .HasColumnName("nobles_noble_id");

                    b.HasKey("ClansClanId", "NoblesNobleId")
                        .HasName("pk_clan_noble");

                    b.HasIndex("NoblesNobleId")
                        .HasDatabaseName("ix_clan_noble_nobles_noble_id");

                    b.ToTable("clan_noble");
                });

            modelBuilder.Entity("ClanResponsibility", b =>
                {
                    b.Property<Guid>("ClansClanId")
                        .HasColumnType("uuid")
                        .HasColumnName("clans_clan_id");

                    b.Property<Guid>("ResponsibilitiesResponsibilityId")
                        .HasColumnType("uuid")
                        .HasColumnName("responsibilities_responsibility_id");

                    b.HasKey("ClansClanId", "ResponsibilitiesResponsibilityId")
                        .HasName("pk_clan_responsibility");

                    b.HasIndex("ResponsibilitiesResponsibilityId")
                        .HasDatabaseName("ix_clan_responsibility_responsibilities_responsibility_id");

                    b.ToTable("clan_responsibility");
                });

            modelBuilder.Entity("KingdomApi.Models.Clan", b =>
                {
                    b.Property<Guid>("ClanId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("clan_id");

                    b.Property<string>("ClanName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("clan_name");

                    b.Property<string>("ClanPurpose")
                        .HasColumnType("text")
                        .HasColumnName("clan_purpose");

                    b.Property<Guid>("KingdomId")
                        .HasColumnType("uuid")
                        .HasColumnName("kingdom_id");

                    b.HasKey("ClanId")
                        .HasName("pk_clans");

                    b.HasIndex("KingdomId")
                        .HasDatabaseName("ix_clans_kingdom_id");

                    b.ToTable("clans");
                });

            modelBuilder.Entity("KingdomApi.Models.Kingdom", b =>
                {
                    b.Property<Guid>("KingdomId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("kingdom_id");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("KingdomName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("kingdom_name");

                    b.HasKey("KingdomId")
                        .HasName("pk_kingdoms");

                    b.ToTable("kingdoms");
                });

            modelBuilder.Entity("KingdomApi.Models.Noble", b =>
                {
                    b.Property<Guid>("NobleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("noble_id");

                    b.Property<string>("Address")
                        .HasColumnType("text")
                        .HasColumnName("address");

                    b.Property<string>("City")
                        .HasColumnType("text")
                        .HasColumnName("city");

                    b.Property<string>("Country")
                        .HasColumnType("text")
                        .HasColumnName("country");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("date_of_birth");

                    b.Property<string>("Department")
                        .HasColumnType("text")
                        .HasColumnName("department");

                    b.Property<string>("EmailAddress")
                        .HasColumnType("text")
                        .HasColumnName("email_address");

                    b.Property<string>("EmployeeId")
                        .HasColumnType("text")
                        .HasColumnName("employee_id");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("full_name");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("varchar(24)")
                        .HasColumnName("gender");

                    b.Property<string>("JobTitle")
                        .HasColumnType("text")
                        .HasColumnName("job_title");

                    b.Property<Guid>("KingdomId")
                        .HasColumnType("uuid")
                        .HasColumnName("kingdom_id");

                    b.Property<string>("OrganizationName")
                        .HasColumnType("text")
                        .HasColumnName("organization_name");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text")
                        .HasColumnName("phone_number");

                    b.Property<int>("PostalCode")
                        .HasColumnType("integer")
                        .HasColumnName("postal_code");

                    b.Property<string>("ReportingManager")
                        .HasColumnType("text")
                        .HasColumnName("reporting_manager");

                    b.Property<string>("State")
                        .HasColumnType("text")
                        .HasColumnName("state");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("username");

                    b.HasKey("NobleId")
                        .HasName("pk_nobles");

                    b.HasIndex("KingdomId")
                        .HasDatabaseName("ix_nobles_kingdom_id");

                    b.ToTable("nobles");
                });

            modelBuilder.Entity("KingdomApi.Models.NobleSecret", b =>
                {
                    b.Property<string>("Username")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("username");

                    b.Property<string>("EmailAddress")
                        .HasColumnType("text")
                        .HasColumnName("email_address");

                    b.Property<Guid>("NobleId")
                        .HasColumnType("uuid")
                        .HasColumnName("noble_id");

                    b.Property<string>("Password")
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.HasKey("Username")
                        .HasName("pk_noble_secrets");

                    b.HasIndex("EmailAddress")
                        .IsUnique()
                        .HasDatabaseName("ix_noble_secrets_email_address");

                    b.HasIndex("NobleId")
                        .IsUnique()
                        .HasDatabaseName("ix_noble_secrets_noble_id");

                    b.HasIndex("Username")
                        .IsUnique()
                        .HasDatabaseName("ix_noble_secrets_username");

                    b.ToTable("noble_secrets");
                });

            modelBuilder.Entity("KingdomApi.Models.Responsibility", b =>
                {
                    b.Property<Guid>("ResponsibilityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("responsibility_id");

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasMaxLength(24)
                        .HasColumnType("character varying(24)")
                        .HasColumnName("action");

                    b.Property<JsonDocument>("Condition")
                        .HasColumnType("jsonb")
                        .HasColumnName("condition");

                    b.Property<string>("Depth")
                        .IsRequired()
                        .HasColumnType("varchar(24)")
                        .HasColumnName("depth");

                    b.Property<string>("Effect")
                        .IsRequired()
                        .HasColumnType("varchar(24)")
                        .HasColumnName("effect");

                    b.Property<Guid>("KingdomId")
                        .HasColumnType("uuid")
                        .HasColumnName("kingdom_id");

                    b.Property<Obligation>("Obligation")
                        .HasColumnType("jsonb")
                        .HasColumnName("obligation");

                    b.Property<short>("Priority")
                        .HasColumnType("smallint")
                        .HasColumnName("priority");

                    b.Property<string>("ResponsibilityName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasColumnName("responsibility_name");

                    b.Property<string>("Target")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasColumnName("target");

                    b.HasKey("ResponsibilityId")
                        .HasName("pk_responsibilities");

                    b.HasIndex("KingdomId")
                        .HasDatabaseName("ix_responsibilities_kingdom_id");

                    b.ToTable("responsibilities");
                });

            modelBuilder.Entity("NobleResponsibility", b =>
                {
                    b.Property<Guid>("NoblesNobleId")
                        .HasColumnType("uuid")
                        .HasColumnName("nobles_noble_id");

                    b.Property<Guid>("ResponsibilitiesResponsibilityId")
                        .HasColumnType("uuid")
                        .HasColumnName("responsibilities_responsibility_id");

                    b.HasKey("NoblesNobleId", "ResponsibilitiesResponsibilityId")
                        .HasName("pk_noble_responsibility");

                    b.HasIndex("ResponsibilitiesResponsibilityId")
                        .HasDatabaseName("ix_noble_responsibility_responsibilities_responsibility_id");

                    b.ToTable("noble_responsibility");
                });

            modelBuilder.Entity("ClanNoble", b =>
                {
                    b.HasOne("KingdomApi.Models.Clan", null)
                        .WithMany()
                        .HasForeignKey("ClansClanId")
                        .HasConstraintName("fk_clan_noble_clans_clans_clan_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KingdomApi.Models.Noble", null)
                        .WithMany()
                        .HasForeignKey("NoblesNobleId")
                        .HasConstraintName("fk_clan_noble_nobles_nobles_noble_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ClanResponsibility", b =>
                {
                    b.HasOne("KingdomApi.Models.Clan", null)
                        .WithMany()
                        .HasForeignKey("ClansClanId")
                        .HasConstraintName("fk_clan_responsibility_clans_clans_clan_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KingdomApi.Models.Responsibility", null)
                        .WithMany()
                        .HasForeignKey("ResponsibilitiesResponsibilityId")
                        .HasConstraintName("fk_clan_responsibility_responsibilities_responsibilities_respo")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("KingdomApi.Models.Clan", b =>
                {
                    b.HasOne("KingdomApi.Models.Kingdom", "Kingdom")
                        .WithMany("Clans")
                        .HasForeignKey("KingdomId")
                        .HasConstraintName("fk_clans_kingdoms_kingdom_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Kingdom");
                });

            modelBuilder.Entity("KingdomApi.Models.Noble", b =>
                {
                    b.HasOne("KingdomApi.Models.Kingdom", "Kingdom")
                        .WithMany("Nobles")
                        .HasForeignKey("KingdomId")
                        .HasConstraintName("fk_nobles_kingdoms_kingdom_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Kingdom");
                });

            modelBuilder.Entity("KingdomApi.Models.NobleSecret", b =>
                {
                    b.HasOne("KingdomApi.Models.Noble", "Noble")
                        .WithOne("NobleSecret")
                        .HasForeignKey("KingdomApi.Models.NobleSecret", "NobleId")
                        .HasConstraintName("fk_noble_secrets_nobles_noble_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Noble");
                });

            modelBuilder.Entity("KingdomApi.Models.Responsibility", b =>
                {
                    b.HasOne("KingdomApi.Models.Kingdom", "Kingdom")
                        .WithMany("Responsibilities")
                        .HasForeignKey("KingdomId")
                        .HasConstraintName("fk_responsibilities_kingdoms_kingdom_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Kingdom");
                });

            modelBuilder.Entity("NobleResponsibility", b =>
                {
                    b.HasOne("KingdomApi.Models.Noble", null)
                        .WithMany()
                        .HasForeignKey("NoblesNobleId")
                        .HasConstraintName("fk_noble_responsibility_nobles_nobles_noble_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KingdomApi.Models.Responsibility", null)
                        .WithMany()
                        .HasForeignKey("ResponsibilitiesResponsibilityId")
                        .HasConstraintName("fk_noble_responsibility_responsibilities_responsibilities_resp")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("KingdomApi.Models.Kingdom", b =>
                {
                    b.Navigation("Clans");

                    b.Navigation("Nobles");

                    b.Navigation("Responsibilities");
                });

            modelBuilder.Entity("KingdomApi.Models.Noble", b =>
                {
                    b.Navigation("NobleSecret");
                });
#pragma warning restore 612, 618
        }
    }
}

﻿// <auto-generated />
using System;
using Issuer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Issuer.Migrations
{
    [DbContext(typeof(ApiDbContext))]
    partial class ApiDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Issuer.Models.Connection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTimeOffset?>("AcceptedConnectionDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Base64QRCode")
                        .HasColumnType("text");

                    b.Property<string>("ConnectionId")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreatedTimeStamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatedUserId")
                        .HasColumnType("uuid");

                    b.Property<int>("PatientId")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("UpdatedTimeStamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UpdatedUserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("PatientId");

                    b.ToTable("Connection");
                });

            modelBuilder.Entity("Issuer.Models.Credential", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTimeOffset?>("AcceptedCredentialDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ConnectionId")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("CreatedTimeStamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatedUserId")
                        .HasColumnType("uuid");

                    b.Property<string>("CredentialDefinitionId")
                        .HasColumnType("text");

                    b.Property<string>("CredentialExchangeId")
                        .HasColumnType("text");

                    b.Property<int>("IdentifierId")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset?>("RevokedCredentialDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("SchemaId")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("UpdatedTimeStamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UpdatedUserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ConnectionId");

                    b.HasIndex("IdentifierId");

                    b.ToTable("Credential");
                });

            modelBuilder.Entity("Issuer.Models.Identifier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTimeOffset>("CreatedTimeStamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatedUserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("Guid")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("UpdatedTimeStamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UpdatedUserId")
                        .HasColumnType("uuid");

                    b.Property<string>("Uri")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Identifier");
                });

            modelBuilder.Entity("Issuer.Models.Patient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTimeOffset>("CreatedTimeStamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatedUserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("GivenNames")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("HPDID")
                        .HasColumnType("character varying(255)")
                        .HasMaxLength(255);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.Property<string>("PreferredFirstName")
                        .HasColumnType("text");

                    b.Property<string>("PreferredLastName")
                        .HasColumnType("text");

                    b.Property<string>("PreferredMiddleName")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("UpdatedTimeStamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UpdatedUserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Patient");
                });

            modelBuilder.Entity("Issuer.Models.Connection", b =>
                {
                    b.HasOne("Issuer.Models.Patient", "Patient")
                        .WithMany("Connections")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Issuer.Models.Credential", b =>
                {
                    b.HasOne("Issuer.Models.Connection", "Connection")
                        .WithMany("Credentials")
                        .HasForeignKey("ConnectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Issuer.Models.Identifier", "Identifier")
                        .WithMany()
                        .HasForeignKey("IdentifierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApi.Models;

namespace WebApi.Migrations
{
    [DbContext(typeof(HospitalContext))]
    [Migration("20181209161023_added-isWorking-field")]
    partial class addedisWorkingfield
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WebApi.Models.Doctors", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("PatronymicName");

                    b.Property<string>("Position");

                    b.HasKey("Id");

                    b.ToTable("Doctors");
                });

            modelBuilder.Entity("WebApi.Models.Documents", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte[]>("Document");

                    b.Property<string>("FileName");

                    b.Property<int?>("PacientId");

                    b.HasKey("Id");

                    b.HasIndex("PacientId")
                        .HasName("IX_PacientId");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("WebApi.Models.Pacients", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BuildingNumber");

                    b.Property<string>("FirstName");

                    b.Property<string>("FlatNumber");

                    b.Property<bool>("IsWorking");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("PacientPhoneNumber");

                    b.Property<int>("PacientType");

                    b.Property<string>("ParentFirstName");

                    b.Property<string>("ParentLastName");

                    b.Property<string>("ParentPatronymicName");

                    b.Property<string>("ParentPhoneNumber");

                    b.Property<string>("PatronymicName");

                    b.Property<string>("Sity")
                        .IsRequired();

                    b.Property<string>("Street")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Pacients");
                });

            modelBuilder.Entity("WebApi.Models.ProgrammGUID", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("GUID");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.ToTable("ProgrammGUID");
                });

            modelBuilder.Entity("WebApi.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("Password");

                    b.Property<string>("PatronymicName");

                    b.Property<Guid>("UserGuid");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("WebApi.Models.VisitLogs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("DoctorId");

                    b.Property<int?>("PacientId");

                    b.Property<DateTime>("VisitDateTime");

                    b.HasKey("Id");

                    b.HasIndex("DoctorId")
                        .HasName("IX_DoctorId");

                    b.HasIndex("PacientId")
                        .HasName("IX_PacientId");

                    b.ToTable("VisitLogs");
                });

            modelBuilder.Entity("WebApi.Models.Documents", b =>
                {
                    b.HasOne("WebApi.Models.Pacients", "Pacient")
                        .WithMany("Documents")
                        .HasForeignKey("PacientId")
                        .HasConstraintName("FK_dbo.Documents_dbo.Pacients_PacientId");
                });

            modelBuilder.Entity("WebApi.Models.VisitLogs", b =>
                {
                    b.HasOne("WebApi.Models.Doctors", "Doctor")
                        .WithMany("VisitLogs")
                        .HasForeignKey("DoctorId")
                        .HasConstraintName("FK_dbo.VisitLogs_dbo.Doctors_DoctorId");

                    b.HasOne("WebApi.Models.Pacients", "Pacient")
                        .WithMany("VisitLogs")
                        .HasForeignKey("PacientId")
                        .HasConstraintName("FK_dbo.VisitLogs_dbo.Pacients_PacientId");
                });
#pragma warning restore 612, 618
        }
    }
}

﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SmartShipping.Persistence;

#nullable disable

namespace SmartShipping.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250417221935_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SmartShipping.Domain.Entities.CarrierCompany", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Capacity")
                        .HasColumnType("integer");

                    b.Property<int>("DeliveryTimeInHours")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SupportedCities")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("CarrierCompanies");
                });

            modelBuilder.Entity("SmartShipping.Domain.Entities.Shipment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AssignedCarrierCompanyId")
                        .HasColumnType("uuid");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("OrderId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PreferredDeliveryTimeInHours")
                        .HasColumnType("integer");

                    b.Property<double>("Weight")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("AssignedCarrierCompanyId");

                    b.ToTable("Shipments");
                });

            modelBuilder.Entity("SmartShipping.Domain.Entities.Shipment", b =>
                {
                    b.HasOne("SmartShipping.Domain.Entities.CarrierCompany", "AssignedCarrierCompany")
                        .WithMany("Shipments")
                        .HasForeignKey("AssignedCarrierCompanyId");

                    b.Navigation("AssignedCarrierCompany");
                });

            modelBuilder.Entity("SmartShipping.Domain.Entities.CarrierCompany", b =>
                {
                    b.Navigation("Shipments");
                });
#pragma warning restore 612, 618
        }
    }
}

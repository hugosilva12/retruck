﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApplication1.Context;

#nullable disable

namespace WebApplication1.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20220828210859_ServiceCoodTable")]
    partial class ServiceCoodTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-preview.4.22229.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("WebApi.Models.Absence", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("absence")
                        .HasColumnType("int");

                    b.Property<DateTime>("date")
                        .HasColumnType("datetime2");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("driverid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("status")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("driverid");

                    b.ToTable("Absence");
                });

            modelBuilder.Entity("WebApi.Models.License", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("driverid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("truck_category")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("driverid");

                    b.ToTable("License");
                });

            modelBuilder.Entity("WebApi.Models.Organization", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("addresses")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("enable")
                        .HasColumnType("bit");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("vatin")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.ToTable("Organization");
                });

            modelBuilder.Entity("WebApi.Models.PathPhoto", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("number")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnOrder(0);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("number"), 1L, 1);

                    b.Property<string>("type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("PathPhoto");
                });

            modelBuilder.Entity("WebApi.Models.ServiceCoord", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("latitude")
                        .HasColumnType("float");

                    b.Property<double>("longitude")
                        .HasColumnType("float");

                    b.Property<Guid>("serviceTransportid")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("id");

                    b.HasIndex("serviceTransportid");

                    b.ToTable("ServiceCoord");
                });

            modelBuilder.Entity("WebApi.Models.ServiceTransport", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("idTransport")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("idTruck")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("kms")
                        .HasColumnType("float");

                    b.Property<double>("profit")
                        .HasColumnType("float");

                    b.Property<int>("status")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.ToTable("Service");
                });

            modelBuilder.Entity("WebApi.Models.Transport", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("capacity")
                        .HasColumnType("float");

                    b.Property<DateTime>("date")
                        .HasColumnType("datetime2");

                    b.Property<string>("destiny")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("liters")
                        .HasColumnType("int");

                    b.Property<string>("origin")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("status")
                        .HasColumnType("int");

                    b.Property<int>("truck_category")
                        .HasColumnType("int");

                    b.Property<Guid>("user_clientid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("value_offered")
                        .HasColumnType("float");

                    b.Property<double>("weight")
                        .HasColumnType("float");

                    b.HasKey("id");

                    b.HasIndex("user_clientid");

                    b.ToTable("Transport");
                });

            modelBuilder.Entity("WebApi.Models.TransportReviewParameters", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("considerTruckBreakDowns")
                        .HasColumnType("int");

                    b.Property<int>("typeAnalysis")
                        .HasColumnType("int");

                    b.Property<double>("valueFuel")
                        .HasColumnType("float");

                    b.Property<double>("valueHoliday")
                        .HasColumnType("float");

                    b.Property<double>("valueSaturday")
                        .HasColumnType("float");

                    b.Property<double>("valueSunday")
                        .HasColumnType("float");

                    b.Property<double>("valueToll")
                        .HasColumnType("float");

                    b.HasKey("id");

                    b.ToTable("TransportReviewParameters");
                });

            modelBuilder.Entity("WebApi.Models.Truck", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("driverid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("fuelConsumption")
                        .HasColumnType("int");

                    b.Property<int>("kms")
                        .HasColumnType("int");

                    b.Property<string>("matricula")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("nextRevision")
                        .HasColumnType("int");

                    b.Property<string>("organization_id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("photoPath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("status")
                        .HasColumnType("int");

                    b.Property<int>("truckCategory")
                        .HasColumnType("int");

                    b.Property<int>("year")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("driverid");

                    b.ToTable("Truck");
                });

            modelBuilder.Entity("WebApi.Models.TruckBreakDowns", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("date")
                        .HasColumnType("datetime2");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("price")
                        .HasColumnType("float");

                    b.Property<Guid>("truckid")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("id");

                    b.HasIndex("truckid");

                    b.ToTable("TruckBreakDowns");
                });

            modelBuilder.Entity("WebApi.Models.User", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("organizationid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("photofilename")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("role")
                        .HasColumnType("int");

                    b.Property<int>("userState")
                        .HasColumnType("int");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("organizationid");

                    b.ToTable("User");
                });

            modelBuilder.Entity("WebApi.Models.Absence", b =>
                {
                    b.HasOne("WebApi.Models.User", "driver")
                        .WithMany()
                        .HasForeignKey("driverid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("driver");
                });

            modelBuilder.Entity("WebApi.Models.License", b =>
                {
                    b.HasOne("WebApi.Models.User", "driver")
                        .WithMany()
                        .HasForeignKey("driverid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("driver");
                });

            modelBuilder.Entity("WebApi.Models.ServiceCoord", b =>
                {
                    b.HasOne("WebApi.Models.ServiceTransport", "serviceTransport")
                        .WithMany()
                        .HasForeignKey("serviceTransportid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("serviceTransport");
                });

            modelBuilder.Entity("WebApi.Models.Transport", b =>
                {
                    b.HasOne("WebApi.Models.User", "user_client")
                        .WithMany()
                        .HasForeignKey("user_clientid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user_client");
                });

            modelBuilder.Entity("WebApi.Models.Truck", b =>
                {
                    b.HasOne("WebApi.Models.User", "driver")
                        .WithMany()
                        .HasForeignKey("driverid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("driver");
                });

            modelBuilder.Entity("WebApi.Models.TruckBreakDowns", b =>
                {
                    b.HasOne("WebApi.Models.Truck", "truck")
                        .WithMany()
                        .HasForeignKey("truckid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("truck");
                });

            modelBuilder.Entity("WebApi.Models.User", b =>
                {
                    b.HasOne("WebApi.Models.Organization", "organization")
                        .WithMany("users")
                        .HasForeignKey("organizationid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("organization");
                });

            modelBuilder.Entity("WebApi.Models.Organization", b =>
                {
                    b.Navigation("users");
                });
#pragma warning restore 612, 618
        }
    }
}

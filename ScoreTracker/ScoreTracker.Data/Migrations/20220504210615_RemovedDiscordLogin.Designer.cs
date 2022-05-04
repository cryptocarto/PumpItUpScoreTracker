﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ScoreTracker.Data.Persistence;

#nullable disable

namespace ScoreTracker.Data.Migrations
{
    [DbContext(typeof(ChartAttemptDbContext))]
    [Migration("20220504210615_RemovedDiscordLogin")]
    partial class RemovedDiscordLogin
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("scores")
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ScoreTracker.Data.Persistence.Entities.BestAttemptEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ChartId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsBroken")
                        .HasColumnType("bit");

                    b.Property<string>("LetterGrade")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("RecordedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ChartId");

                    b.HasIndex("UserId", "ChartId");

                    b.ToTable("BestAttempt", "scores");
                });

            modelBuilder.Entity("ScoreTracker.Data.Persistence.Entities.ChartEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<Guid>("SongId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Level");

                    b.HasIndex("SongId");

                    b.HasIndex("Type");

                    b.ToTable("Chart", "scores");
                });

            modelBuilder.Entity("ScoreTracker.Data.Persistence.Entities.ExternalLoginEntity", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("ExternalId")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LoginProvider", "ExternalId");

                    b.HasIndex("UserId");

                    b.ToTable("ExternalLogin", "scores");
                });

            modelBuilder.Entity("ScoreTracker.Data.Persistence.Entities.SongEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.ToTable("Song", "scores");
                });

            modelBuilder.Entity("ScoreTracker.Data.Persistence.Entities.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("User", "scores");
                });

            modelBuilder.Entity("ScoreTracker.Data.Persistence.Entities.BestAttemptEntity", b =>
                {
                    b.HasOne("ScoreTracker.Data.Persistence.Entities.ChartEntity", null)
                        .WithMany()
                        .HasForeignKey("ChartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ScoreTracker.Data.Persistence.Entities.UserEntity", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ScoreTracker.Data.Persistence.Entities.ChartEntity", b =>
                {
                    b.HasOne("ScoreTracker.Data.Persistence.Entities.SongEntity", null)
                        .WithMany()
                        .HasForeignKey("SongId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ScoreTracker.Data.Persistence.Entities.ExternalLoginEntity", b =>
                {
                    b.HasOne("ScoreTracker.Data.Persistence.Entities.UserEntity", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

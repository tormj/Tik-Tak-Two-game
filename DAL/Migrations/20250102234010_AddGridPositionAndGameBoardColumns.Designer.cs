﻿// <auto-generated />
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DAL.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250102234010_AddGridPositionAndGameBoardColumns")]
    partial class AddGridPositionAndGameBoardColumns
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("Domain.Configuration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BoardHeight")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BoardWidth")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GridSize")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MovePieceAfterNMoves")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<int>("WinCondition")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Configurations");
                });

            modelBuilder.Entity("Domain.SaveGame", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ConfigurationId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CreatedAtDateTime")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("GameBoard")
                        .IsRequired()
                        .HasMaxLength(10240)
                        .HasColumnType("TEXT");

                    b.Property<int>("GridPositionX")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GridPositionY")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(10240)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ConfigurationId");

                    b.ToTable("SaveGames");
                });

            modelBuilder.Entity("Domain.SaveGame", b =>
                {
                    b.HasOne("Domain.Configuration", "Configuration")
                        .WithMany("SaveGames")
                        .HasForeignKey("ConfigurationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Configuration");
                });

            modelBuilder.Entity("Domain.Configuration", b =>
                {
                    b.Navigation("SaveGames");
                });
#pragma warning restore 612, 618
        }
    }
}

﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using txts.Database;

#nullable disable

namespace txts.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.10");

            modelBuilder.Entity("txts.Types.Entities.AdminUserEntity", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("UserId");

                    b.ToTable("AdminUsers");
                });

            modelBuilder.Entity("txts.Types.Entities.BanEntity", b =>
                {
                    b.Property<int>("BanId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("PageId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("BanId");

                    b.HasIndex("PageId");

                    b.ToTable("Bans");
                });

            modelBuilder.Entity("txts.Types.Entities.PageEntity", b =>
                {
                    b.Property<int>("PageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Contents")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsBanned")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsVerified")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Secret")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("PageId");

                    b.ToTable("Pages");
                });

            modelBuilder.Entity("txts.Types.Entities.WebSessionEntity", b =>
                {
                    b.Property<int>("WebSessionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("WebSessionId");

                    b.HasIndex("UserId");

                    b.ToTable("WebSessions");
                });

            modelBuilder.Entity("txts.Types.Entities.BanEntity", b =>
                {
                    b.HasOne("txts.Types.Entities.PageEntity", "Page")
                        .WithMany()
                        .HasForeignKey("PageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Page");
                });

            modelBuilder.Entity("txts.Types.Entities.WebSessionEntity", b =>
                {
                    b.HasOne("txts.Types.Entities.AdminUserEntity", "AdminUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AdminUser");
                });
#pragma warning restore 612, 618
        }
    }
}

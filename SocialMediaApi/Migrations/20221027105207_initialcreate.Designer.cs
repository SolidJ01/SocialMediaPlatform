﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SocialMediaApi.Data;

#nullable disable

namespace SocialMediaApi.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20221027105207_initialcreate")]
    partial class initialcreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.10");

            modelBuilder.Entity("SocialMediaApi.Data.LoginToken", b =>
                {
                    b.Property<string>("HashedValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("DeviceIdiom")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("DeviceManufacturer")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("DeviceModel")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("DevicePlatform")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("DeviceType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastAccessed")
                        .HasColumnType("TEXT");

                    b.Property<int>("OwnerId")
                        .HasColumnType("INTEGER");

                    b.HasKey("HashedValue");

                    b.HasIndex("OwnerId");

                    b.ToTable("LoginTokens");
                });

            modelBuilder.Entity("SocialMediaApi.Data.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("HashedPassword")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ProfilePictureSource")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("UserUser", b =>
                {
                    b.Property<int>("FollowersId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FollowingId")
                        .HasColumnType("INTEGER");

                    b.HasKey("FollowersId", "FollowingId");

                    b.HasIndex("FollowingId");

                    b.ToTable("UserUser");
                });

            modelBuilder.Entity("SocialMediaApi.Data.LoginToken", b =>
                {
                    b.HasOne("SocialMediaApi.Data.User", "Owner")
                        .WithMany("LoginTokens")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("UserUser", b =>
                {
                    b.HasOne("SocialMediaApi.Data.User", null)
                        .WithMany()
                        .HasForeignKey("FollowersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SocialMediaApi.Data.User", null)
                        .WithMany()
                        .HasForeignKey("FollowingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SocialMediaApi.Data.User", b =>
                {
                    b.Navigation("LoginTokens");
                });
#pragma warning restore 612, 618
        }
    }
}

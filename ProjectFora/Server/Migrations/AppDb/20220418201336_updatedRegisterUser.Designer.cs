﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjectFora.Server.Data;

#nullable disable

namespace ProjectFora.Server.Migrations.AppDb
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20220418201336_updatedRegisterUser")]
    partial class updatedRegisterUser
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ProjectFora.Shared.InterestModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Interests");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Games"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Sports"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Politics"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Religion"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Design"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Garden"
                        },
                        new
                        {
                            Id = 7,
                            Name = "Technology"
                        },
                        new
                        {
                            Id = 8,
                            Name = "Pets"
                        });
                });

            modelBuilder.Entity("ProjectFora.Shared.MessageModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("MessageCreated")
                        .HasColumnType("datetime2");

                    b.Property<int>("ThreadId")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ThreadId");

                    b.HasIndex("UserId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("ProjectFora.Shared.ThreadModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("InterestId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("InterestId");

                    b.HasIndex("UserId");

                    b.ToTable("Threads");
                });

            modelBuilder.Entity("ProjectFora.Shared.UserInterestModel", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("InterestId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "InterestId");

                    b.HasIndex("InterestId");

                    b.ToTable("UserInterests");
                });

            modelBuilder.Entity("ProjectFora.Shared.UserModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("Banned")
                        .HasColumnType("bit");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ProjectFora.Shared.InterestModel", b =>
                {
                    b.HasOne("ProjectFora.Shared.UserModel", "User")
                        .WithMany("Interests")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("User");
                });

            modelBuilder.Entity("ProjectFora.Shared.MessageModel", b =>
                {
                    b.HasOne("ProjectFora.Shared.ThreadModel", "Thread")
                        .WithMany("Messages")
                        .HasForeignKey("ThreadId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectFora.Shared.UserModel", "User")
                        .WithMany("Messages")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Thread");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ProjectFora.Shared.ThreadModel", b =>
                {
                    b.HasOne("ProjectFora.Shared.InterestModel", "Interest")
                        .WithMany("Threads")
                        .HasForeignKey("InterestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectFora.Shared.UserModel", "User")
                        .WithMany("Threads")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Interest");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ProjectFora.Shared.UserInterestModel", b =>
                {
                    b.HasOne("ProjectFora.Shared.InterestModel", "Interest")
                        .WithMany("UserInterests")
                        .HasForeignKey("InterestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectFora.Shared.UserModel", "User")
                        .WithMany("UserInterests")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Interest");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ProjectFora.Shared.InterestModel", b =>
                {
                    b.Navigation("Threads");

                    b.Navigation("UserInterests");
                });

            modelBuilder.Entity("ProjectFora.Shared.ThreadModel", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("ProjectFora.Shared.UserModel", b =>
                {
                    b.Navigation("Interests");

                    b.Navigation("Messages");

                    b.Navigation("Threads");

                    b.Navigation("UserInterests");
                });
#pragma warning restore 612, 618
        }
    }
}

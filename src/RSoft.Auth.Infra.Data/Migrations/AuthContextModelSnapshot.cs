﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RSoft.Auth.Infra.Data;

namespace RSoft.Auth.Infra.Data.Migrations
{
    [DbContext(typeof(AuthContext))]
    partial class AuthContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("RSoft.Auth.Infra.Data.Entities.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("ChangedBy")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("ChangedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnName("Description")
                        .HasColumnType("varchar(150) CHARACTER SET utf8mb4")
                        .HasMaxLength(150)
                        .IsUnicode(false);

                    b.Property<ulong>("IsActive")
                        .HasColumnType("bit");

                    b.Property<ulong>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(0ul);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<Guid>("ScopeId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("ChangedBy")
                        .HasName("IX_Role_ChangedBy");

                    b.HasIndex("ChangedOn")
                        .HasName("IX_Role_ChangedOn");

                    b.HasIndex("CreatedBy")
                        .HasName("IX_Role_CreatedBy");

                    b.HasIndex("CreatedOn")
                        .HasName("IX_Role_CreatedOn");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasName("AK_Role_Name");

                    b.HasIndex("ScopeId")
                        .HasName("IX_Role_ScopeId");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("RSoft.Auth.Infra.Data.Entities.Scope", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("AccessKey")
                        .HasColumnName("AccessKey")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("ChangedBy")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("ChangedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<ulong>("IsActive")
                        .HasColumnType("bit");

                    b.Property<ulong>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(0ul);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.HasIndex("AccessKey")
                        .IsUnique()
                        .HasName("AK_Scope_AccessKey");

                    b.HasIndex("ChangedBy")
                        .HasName("IX_Scope_ChangedBy");

                    b.HasIndex("ChangedOn")
                        .HasName("IX_Scope_ChangedOn");

                    b.HasIndex("CreatedBy")
                        .HasName("IX_Scope_CreatedBy");

                    b.HasIndex("CreatedOn")
                        .HasName("IX_Scope_CreatedOn");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasName("AK_Scope_Name");

                    b.ToTable("Scope");
                });

            modelBuilder.Entity("RSoft.Auth.Infra.Data.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("BornDate")
                        .IsRequired()
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("ChangedBy")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("ChangedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnName("Email")
                        .HasColumnType("varchar(254) CHARACTER SET utf8mb4")
                        .HasMaxLength(254)
                        .IsUnicode(false);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnName("FirstName")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<ulong>("IsActive")
                        .HasColumnType("bit");

                    b.Property<ulong>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(0ul);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnName("LastName")
                        .HasColumnType("varchar(100) CHARACTER SET utf8mb4")
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<int>("Type")
                        .HasColumnName("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ChangedBy")
                        .HasName("IX_User_ChangedBy");

                    b.HasIndex("ChangedOn")
                        .HasName("IX_User_ChangedOn");

                    b.HasIndex("CreatedBy")
                        .HasName("IX_User_CreatedBy");

                    b.HasIndex("CreatedOn")
                        .HasName("IX_User_CreatedOn");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasName("AK_User_Email");

                    b.HasIndex("FirstName", "LastName")
                        .HasName("IX_User_FullName");

                    b.ToTable("User");
                });

            modelBuilder.Entity("RSoft.Auth.Infra.Data.Entities.UserCredential", b =>
                {
                    b.Property<Guid?>("UserId")
                        .HasColumnType("char(36)");

                    b.Property<ulong>("ChangeCredentials")
                        .HasColumnName("ChangeCredentials")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .HasColumnName("Password")
                        .HasColumnType("varchar(32) CHARACTER SET utf8mb4")
                        .HasMaxLength(32)
                        .IsUnicode(false);

                    b.Property<Guid?>("UserKey")
                        .HasColumnName("UserKey")
                        .HasColumnType("char(36)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnName("Username")
                        .HasColumnType("varchar(254) CHARACTER SET utf8mb4")
                        .HasMaxLength(254)
                        .IsUnicode(false);

                    b.HasKey("UserId");

                    b.HasIndex("UserKey")
                        .IsUnique()
                        .HasName("AK_UserCredential_UserKey");

                    b.HasIndex("Username")
                        .IsUnique()
                        .HasName("AK_UserCredential_Username");

                    b.ToTable("UserCredential");
                });

            modelBuilder.Entity("RSoft.Auth.Infra.Data.Entities.UserCredentialToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("ExpiresOn")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("FirstAccess")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserCredentialToken");
                });

            modelBuilder.Entity("RSoft.Auth.Infra.Data.Entities.UserRole", b =>
                {
                    b.Property<Guid?>("UserId")
                        .HasColumnName("UserId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("RoleId")
                        .HasColumnName("RoleId")
                        .HasColumnType("char(36)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("RSoft.Auth.Infra.Data.Entities.UserScope", b =>
                {
                    b.Property<Guid?>("UserId")
                        .HasColumnName("UserId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("ScopeId")
                        .HasColumnName("ScopeId")
                        .HasColumnType("char(36)");

                    b.HasKey("UserId", "ScopeId");

                    b.HasIndex("ScopeId");

                    b.ToTable("UserScope");
                });

            modelBuilder.Entity("RSoft.Auth.Infra.Data.Entities.Role", b =>
                {
                    b.HasOne("RSoft.Auth.Infra.Data.Entities.User", "ChangedAuthor")
                        .WithMany("ChangedRoles")
                        .HasForeignKey("ChangedBy")
                        .HasConstraintName("FK_Role_ChangedAuthor")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("RSoft.Auth.Infra.Data.Entities.User", "CreatedAuthor")
                        .WithMany("CreatedRoles")
                        .HasForeignKey("CreatedBy")
                        .HasConstraintName("FK_Role_CreatedAuthor")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("RSoft.Auth.Infra.Data.Entities.Scope", "Scope")
                        .WithMany("Roles")
                        .HasForeignKey("ScopeId")
                        .HasConstraintName("FK_Role_Scope")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RSoft.Auth.Infra.Data.Entities.Scope", b =>
                {
                    b.HasOne("RSoft.Auth.Infra.Data.Entities.User", "ChangedAuthor")
                        .WithMany("ChangedScopes")
                        .HasForeignKey("ChangedBy")
                        .HasConstraintName("FK_Scope_ChangedAuthor")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("RSoft.Auth.Infra.Data.Entities.User", "CreatedAuthor")
                        .WithMany("CreatedScopes")
                        .HasForeignKey("CreatedBy")
                        .HasConstraintName("FK_Scope_CreatedAuthor")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("RSoft.Auth.Infra.Data.Entities.User", b =>
                {
                    b.HasOne("RSoft.Auth.Infra.Data.Entities.User", "ChangedAuthor")
                        .WithMany("ChangedUsers")
                        .HasForeignKey("ChangedBy")
                        .HasConstraintName("FK_User_ChangedAuthor")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("RSoft.Auth.Infra.Data.Entities.User", "CreatedAuthor")
                        .WithMany("CreatedUsers")
                        .HasForeignKey("CreatedBy")
                        .HasConstraintName("FK_User_CreatedAuthor")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("RSoft.Auth.Infra.Data.Entities.UserCredential", b =>
                {
                    b.HasOne("RSoft.Auth.Infra.Data.Entities.User", "User")
                        .WithOne("Credential")
                        .HasForeignKey("RSoft.Auth.Infra.Data.Entities.UserCredential", "UserId")
                        .HasConstraintName("FK_UserCredential_User")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RSoft.Auth.Infra.Data.Entities.UserCredentialToken", b =>
                {
                    b.HasOne("RSoft.Auth.Infra.Data.Entities.User", "User")
                        .WithMany("Tokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RSoft.Auth.Infra.Data.Entities.UserRole", b =>
                {
                    b.HasOne("RSoft.Auth.Infra.Data.Entities.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK_UserRole_Role")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RSoft.Auth.Infra.Data.Entities.User", "User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_UserRole_User")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RSoft.Auth.Infra.Data.Entities.UserScope", b =>
                {
                    b.HasOne("RSoft.Auth.Infra.Data.Entities.Scope", "Scope")
                        .WithMany("Users")
                        .HasForeignKey("ScopeId")
                        .HasConstraintName("FK_UserScope_Scope")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RSoft.Auth.Infra.Data.Entities.User", "User")
                        .WithMany("Scopes")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_UserScope_User")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

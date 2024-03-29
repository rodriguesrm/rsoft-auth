﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RSoft.Auth.Infra.Data.Entities;
using System;

namespace RSoft.Auth.Infra.Data.Configurations
{

    /// <summary>
    /// Role entity table configuration
    /// </summary>
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {

        ///<inheritdoc/>
        public void Configure(EntityTypeBuilder<Role> builder)
        {

            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            builder.ToTable(nameof(Role));

            #region PK

            builder.HasKey(k => k.Id);

            #endregion

            #region Columns

            builder.Property(c => c.Name)
                .HasColumnName(nameof(Role.Name))
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(c => c.Description)
                .HasColumnName(nameof(Role.Description))
                .HasMaxLength(150)
                .IsUnicode(false)
                .IsRequired();

            #endregion

            #region FKs

            builder.HasOne(o => o.CreatedAuthor)
                .WithMany(d => d.CreatedRoles)
                .HasForeignKey(fk => fk.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName($"FK_{nameof(Role)}_CreatedAuthor");

            builder.HasOne(o => o.ChangedAuthor)
                .WithMany(d => d.ChangedRoles)
                .HasForeignKey(fk => fk.ChangedBy)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName($"FK_{nameof(Role)}_ChangedAuthor");

            builder.HasOne(o => o.AppClient)
                .WithMany(d => d.Roles)
                .HasForeignKey(fk => fk.ApplicationClientId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName($"FK_{nameof(Role)}_{nameof(AppClient)}");

            #endregion

            #region Indexes

            builder
                .HasIndex(i => i.Name)
                .HasDatabaseName($"AK_{nameof(Role)}_{nameof(Role.Name)}")
                .IsUnique();

            builder
                .HasIndex(i => i.ApplicationClientId)
                .HasDatabaseName($"IX_{nameof(Role)}_{nameof(Role.ApplicationClientId)}");

            #endregion

        }
    }

}

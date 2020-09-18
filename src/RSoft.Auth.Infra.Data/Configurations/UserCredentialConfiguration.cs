﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RSoft.Auth.Infra.Data.Entities;
using System;

namespace RSoft.Auth.Infra.Data.Configurations
{

    /// <summary>
    /// User Credential entity table configuration
    /// </summary>
    public class UserCredentialConfiguration : IEntityTypeConfiguration<UserCredential>
    {

        ///<inheritdoc/>
        public void Configure(EntityTypeBuilder<UserCredential> builder)
        {

            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            builder.ToTable(nameof(UserCredential));

            #region PK

            builder.HasKey(k => k.UserId);

            #endregion

            #region Columns

            builder.Property(c => c.Username)
                .HasColumnName(nameof(UserCredential.Username))
                .HasMaxLength(254)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(c => c.UserKey)
                .HasColumnName(nameof(UserCredential.UserKey));

            builder.Property(c => c.Password)
                .HasColumnName(nameof(UserCredential.Password))
                .HasMaxLength(32)
                .IsUnicode(false);

            #endregion

            #region FKs

            builder.HasOne(o => o.User)
                .WithOne(d => d.Credential)
                .HasForeignKey<UserCredential>(fk => fk.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName($"FK_{nameof(UserCredential)}_{nameof(User)}");

            #endregion

            #region Indexes

            builder.HasIndex(c => c.Username)
                .HasName($"AK_{nameof(UserCredential)}_{nameof(UserCredential.Username)}")
                .IsUnique();

            builder.HasIndex(c => c.UserKey)
                .HasName($"AK_{nameof(UserCredential)}_{nameof(UserCredential.UserKey)}")
                .IsUnique();

            #endregion

        }

    }

}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RSoft.Auth.Infra.Data.Entities;
using System;
using System.Reflection.Metadata;

namespace RSoft.Auth.Infra.Data.Configurations
{

    /// <summary>
    /// User entity table configuration
    /// </summary>
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {

        ///<inheritdoc/>
        public void Configure(EntityTypeBuilder<User> builder)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            builder.ToTable(nameof(User));

            #region PK

            builder.HasKey(k => k.Id);

            #endregion

            #region Columns

            builder.Property(c => c.Document)
                .HasColumnName(nameof(User.Document))
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(c => c.FirstName)
                .HasColumnName(nameof(User.FirstName))
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(c => c.LastName)
                .HasColumnName(nameof(User.LastName))
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(c => c.BornDate)
                .IsRequired();

            builder.Property(c => c.Email)
                .HasColumnName(nameof(User.Email))
                .HasMaxLength(254)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(c => c.Type)
                .HasColumnName(nameof(User.Type))
                .IsRequired();

            #endregion

            #region FKs

            builder.HasOne(o => o.CreatedAuthor)
                .WithMany(d => d.CreatedUsers)
                .HasForeignKey(fk => fk.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName($"FK_{nameof(User)}_CreatedAuthor");

            builder.HasOne(o => o.ChangedAuthor)
                .WithMany(d => d.ChangedUsers)
                .HasForeignKey(fk => fk.ChangedBy)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName($"FK_{nameof(User)}_ChangedAuthor");

            #endregion

            #region Indexes

            builder.HasIndex(i => i.Document, $"AK_{nameof(User)}_{nameof(User.Document)}")
                .IsUnique();

            builder.HasIndex(i => i.Email, $"AK_{nameof(User)}_{nameof(User.Email)}")
                .IsUnique();

            builder.HasIndex(i => new { i.FirstName, i.LastName }, $"IX_{nameof(User)}_FullName");

            #endregion

        }

    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RSofth.Auth.Domain.Entities;
using System;

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

            builder.Property(c => c.FirstName)
                .HasColumnName(nameof(User.FirstName))
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

            #endregion

            #region FKs

            builder.HasOne(o => o.CreatedAuthor)
                .WithMany(d => d.CreatedUsers)
                .HasForeignKey(fk => fk.Id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName($"FK_{nameof(User)}_CreatedAuthor");

            builder.HasOne(o => o.ChangedAuthor)
                .WithMany(d => d.ChangedUsers)
                .HasForeignKey(fk => fk.Id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName($"FK_{nameof(User)}_ChangedAuthor");

            #endregion

            #region Indexes

            builder.HasIndex(i => i.Email)
                .HasName($"AK_{nameof(User)}_{nameof(User.Email)}")
                .IsUnique();

            builder.HasIndex(i => new { i.FirstName, i.LastName })
                .HasName($"IX_{nameof(User)}_FullName");

            #endregion

        }

    }
}

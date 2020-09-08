using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RSofth.Auth.Domain.Entities;
using System;

namespace RSoft.Auth.Infra.Data.Configuration
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

            #region Campos

            builder.Property(c => c.FirstName)
                .HasColumnName(nameof(User.FirstName))
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

            #endregion

            #region FKs

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

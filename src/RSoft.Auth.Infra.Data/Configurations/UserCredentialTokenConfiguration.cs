using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RSoft.Auth.Infra.Data.Entities;
using System;

namespace RSoft.Auth.Infra.Data.Configurations
{

    /// <summary>
    /// User Credential Token entity table configuration
    /// </summary>
    public class UserCredentialTokenConfiguration : IEntityTypeConfiguration<UserCredentialToken>
    {

        ///<inheritdoc/>
        public void Configure(EntityTypeBuilder<UserCredentialToken> builder)
        {

            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            builder.ToTable(nameof(UserCredentialToken));

            #region PK

            builder.HasKey(k => k.Id);

            #endregion

            #region Columns

            builder.Property(c => c.UserId)
                .HasColumnName(nameof(UserCredentialToken.UserId))
                .IsRequired();

            builder.Property(c => c.CreatedAt)
                .HasColumnName(nameof(UserCredentialToken.CreatedAt))
                .IsRequired();

            builder.Property(c => c.ExpiresOn)
                .HasColumnName(nameof(UserCredentialToken.ExpiresOn))
                .IsRequired();

            builder.Property(c => c.FirstAccess)
                .HasColumnName(nameof(UserCredentialToken.FirstAccess))
                .HasColumnType("bit")
                .IsRequired();

            #endregion

            #region FKs

            builder.HasOne(o => o.User)
                .WithMany(d => d.Tokens)
                .HasForeignKey(fk => fk.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName($"FK_{nameof(UserCredentialToken)}_User");

            #endregion

            #region Indexes

            builder
                .HasIndex(i => i.UserId)
                .HasDatabaseName($"IX_{nameof(UserCredentialToken)}_{nameof(UserCredentialToken.UserId)}");

            #endregion

        }
    }

}

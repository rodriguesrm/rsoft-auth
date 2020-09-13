using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RSoft.Auth.Infra.Data.Entities;
using System;

namespace RSoft.Auth.Infra.Data.Configurations
{

    /// <summary>
    /// User Scope entity table configuration
    /// </summary>
    public class UserScopeConfiguration : IEntityTypeConfiguration<UserScope>
    {

        ///<inheritdoc/>
        public void Configure(EntityTypeBuilder<UserScope> builder)
        {

            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            builder.ToTable(nameof(UserScope));

            #region PK

            builder.HasKey(k => new { k.UserId, k.ScopeId });

            #endregion

            #region Columns

            builder.Property(c => c.UserId)
                .HasColumnName(nameof(UserScope.UserId))
                .IsRequired();

            builder.Property(c => c.ScopeId)
                .HasColumnName(nameof(UserScope.ScopeId))
                .IsRequired();

            #endregion

            #region Indexes

            builder.HasOne(o => o.User)
                .WithMany(d => d.Scopes)
                .HasForeignKey(fk => fk.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName($"FK_{nameof(UserScope)}_{nameof(User)}");

            builder.HasOne(o => o.Scope)
                .WithMany(d => d.Users)
                .HasForeignKey(fk => fk.ScopeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName($"FK_{nameof(UserScope)}_{nameof(Scope)}");

            #endregion

        }

    }

}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RSoft.Auth.Infra.Data.Entities;
using System;

namespace RSoft.Auth.Infra.Data.Configurations
{

    /// <summary>
    /// User role entity table configuration
    /// </summary>
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {

        ///<inheritdoc/>
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {

            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            builder.ToTable(nameof(UserRole));

            #region PK

            builder.HasKey(k => new { k.UserId, k.RoleId });

            #endregion

            #region Columns

            builder.Property(c => c.UserId)
                .HasColumnName(nameof(UserRole.UserId))
                .IsRequired();

            builder.Property(c => c.RoleId)
                .HasColumnName(nameof(UserRole.RoleId))
                .IsRequired();

            #endregion

            #region Indexes

            builder.HasOne(o => o.User)
                .WithMany(d => d.Roles)
                .HasForeignKey(fk => fk.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName($"FK_{nameof(UserRole)}_{nameof(User)}");

            builder.HasOne(o => o.Role)
                .WithMany(d => d.Users)
                .HasForeignKey(fk => fk.RoleId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName($"FK_{nameof(UserRole)}_{nameof(Role)}");

            #endregion

        }

    }

}

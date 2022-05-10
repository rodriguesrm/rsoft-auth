using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RSoft.Auth.Infra.Data.Entities;
using System;

namespace RSoft.Auth.Infra.Data.Configurations
{

    /// <summary>
    /// User App-Client entity table configuration
    /// </summary>
    public class UserAppClientConfiguration : IEntityTypeConfiguration<UserAppClient>
    {

        ///<inheritdoc/>
        public void Configure(EntityTypeBuilder<UserAppClient> builder)
        {

            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            builder.ToTable(nameof(UserAppClient));

            #region PK

            builder.HasKey(k => new { k.UserId, k.AppClientId });

            #endregion

            #region Columns

            builder.Property(c => c.UserId)
                .HasColumnName(nameof(UserAppClient.UserId))
                .IsRequired();

            builder.Property(c => c.AppClientId)
                .HasColumnName(nameof(UserAppClient.AppClientId))
                .IsRequired();

            #endregion

            #region Indexes

            builder.HasOne(o => o.User)
                .WithMany(d => d.ApplicationClients)
                .HasForeignKey(fk => fk.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName($"FK_{nameof(UserAppClient)}_{nameof(User)}");

            builder.HasOne(o => o.ApplicationClient)
                .WithMany(d => d.Users)
                .HasForeignKey(fk => fk.AppClientId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName($"FK_{nameof(UserAppClient)}_{nameof(AppClient)}");

            #endregion

        }

    }

}

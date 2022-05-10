using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RSoft.Auth.Infra.Data.Entities;
using System;

namespace RSoft.Auth.Infra.Data.Configurations
{

    /// <summary>
    /// Application-Client entity table configuration
    /// </summary>
    public class AppClientConfiguration : IEntityTypeConfiguration<AppClient>
    {
        
        ///<inheritdoc/>
        public void Configure(EntityTypeBuilder<AppClient> builder)
        {

            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            builder.ToTable(nameof(AppClient));

            #region PK

            builder.HasKey(k => k.Id);

            #endregion

            #region Columns

            builder.Property(c => c.Name)
                .HasColumnName(nameof(AppClient.Name))
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(c => c.AccessKey)
                .HasColumnName(nameof(AppClient.AccessKey))
                .IsRequired();

            builder.Property(c => c.AllowLogin)
                .HasColumnName(nameof(AppClient.AllowLogin))
                .HasColumnType("bit")
                .IsRequired();

            #endregion

            #region FKs

            builder.HasOne(o => o.CreatedAuthor)
                .WithMany(d => d.CreatedAppClients)
                .HasForeignKey(fk => fk.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName($"FK_{nameof(AppClient)}_CreatedAuthor");

            builder.HasOne(o => o.ChangedAuthor)
                .WithMany(d => d.ChangedAppClients)
                .HasForeignKey(fk => fk.ChangedBy)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName($"FK_{nameof(AppClient)}_ChangedAuthor");

            #endregion

            #region Indexes

            builder
                .HasIndex(i => i.Name)
                .HasDatabaseName($"AK_{nameof(AppClient)}_{nameof(AppClient.Name)}")
                .IsUnique();

            builder
                .HasIndex(i => i.AccessKey)
                .HasDatabaseName($"AK_{nameof(AppClient)}_{nameof(AppClient.AccessKey)}")
                .IsUnique();

            #endregion

        }
    }

}

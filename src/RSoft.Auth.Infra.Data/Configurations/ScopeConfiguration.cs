using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RSoft.Auth.Infra.Data.Entities;
using System;

namespace RSoft.Auth.Infra.Data.Configurations
{

    /// <summary>
    /// Scope entity table configuration
    /// </summary>
    public class ScopeConfiguration : IEntityTypeConfiguration<Scope>
    {
        ///<inheritdoc/>
        public void Configure(EntityTypeBuilder<Scope> builder)
        {

            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            builder.ToTable(nameof(Scope));

            #region PK

            builder.HasKey(k => k.Id);

            #endregion

            #region Columns

            builder.Property(c => c.Name)
                .HasColumnName(nameof(Scope.Name))
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(c => c.AccessKey)
                .HasColumnName(nameof(Scope.AccessKey))
                .IsRequired();

            #endregion

            #region FKs

            builder.HasOne(o => o.CreatedAuthor)
                .WithMany(d => d.CreatedScopes)
                .HasForeignKey(fk => fk.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName($"FK_{nameof(Scope)}_CreatedAuthor");

            builder.HasOne(o => o.ChangedAuthor)
                .WithMany(d => d.ChangedScopes)
                .HasForeignKey(fk => fk.ChangedBy)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName($"FK_{nameof(Scope)}_ChangedAuthor");

            #endregion

            #region Indexes

            builder.HasIndex(i => i.Name)
                .HasName($"AK_{nameof(Scope)}_{nameof(Scope.Name)}")
                .IsUnique();

            builder.HasIndex(i => i.AccessKey)
                .HasName($"AK_{nameof(Scope)}_{nameof(Scope.AccessKey)}")
                .IsUnique();

            #endregion

        }
    }

}

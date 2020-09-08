using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RSofth.Auth.Domain.Entities;
using System;

namespace RSoft.Auth.Infra.Data.Configuration
{

    /// <summary>
    /// Role entity table configuration
    /// </summary>
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        ///<inheritdoc/>
        public void Configure(EntityTypeBuilder<Role> builder)
        {

            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            builder.ToTable(nameof(Role));

        }
    }

}

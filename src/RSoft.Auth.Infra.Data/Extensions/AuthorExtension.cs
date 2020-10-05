using System;
using RSoft.Auth.Infra.Data.Entities;
using RSoft.Framework.Domain.Entities;
using RSoft.Framework.Domain.ValueObjects;
using RSoft.Framework.Infra.Data;

namespace RSoft.Auth.Infra.Data.Extensions
{

    /// <summary>
    /// Provides author map extensions method
    /// </summary>
    public static class AuthorExtension
    {

        /// <summary>
        /// Add mapped authors in domain entity from table entity
        /// </summary>
        /// <param name="entity">Domain entity object</param>
        /// <param name="table">Table entity object</param>
        public static void MapAuthor(this IAuditAuthor<Guid> entity, IAuditNavigation<Guid, User> table)
        {
            entity.CreatedAuthor = new Author<Guid>(table.CreatedBy, table.CreatedAuthor?.GetFullName());
            if (table.ChangedBy != null)
                entity.ChangedAuthor = new AuthorNullable<Guid>(table.ChangedBy.Value, table.ChangedAuthor.GetFullName());
        }

    }

}

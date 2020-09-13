using RSoft.Framework.Application.Model;
using RSoft.Auth.Domain.Entities;
using RSoft.Framework.Infra.Data;

namespace RSoft.Auth.Application.Model.Extensions
{

    /// <summary>
    /// Provides methods to map author data from entity to dto
    /// </summary>
    public static class Author
    {

        /// <summary>
        /// Map author data
        /// </summary>
        /// <param name="dto">Data transport object instance</param>
        /// <param name="entity">Entity object instance</param>
        public static void Map<TKey>(IAuditDto<TKey> dto, IAuditNavigation<TKey, User> entity)
            where TKey : struct
        {

            AuditAuthor<TKey> createdBy = new AuditAuthor<TKey>(entity.CreatedOn, entity.CreatedBy, entity.CreatedAuthor?.GetFullName());
            AuditAuthor<TKey> changedBy = null;
            if (entity.ChangedAuthor != null)
                changedBy = new AuditAuthor<TKey>(entity.ChangedOn.Value, entity.ChangedBy.Value, entity.ChangedAuthor?.GetFullName());
            dto.CreatedBy = createdBy;
            dto.ChangedBy = changedBy;

        }

    }
}

using RSoft.Framework.Application.Dto;
using RSoft.Framework.Application.Model;
using RSoft.Framework.Domain.Entities;

namespace RSoft.Auth.Application.Model.Extensions
{

    /// <summary>
    /// Provides methods to map author data from entity to dto
    /// </summary>
    public static class AuthorMap
    {

        /// <summary>
        /// Map author data
        /// </summary>
        /// <param name="dto">Data transport object instance</param>
        /// <param name="entity">Entity object instance</param>
        public static void Map<TKey>(IAuditDto<TKey> dto, IAuditAuthor<TKey> entity)
            where TKey : struct
        {

            AuditAuthor<TKey> createdBy = null;
            AuditAuthor<TKey> changedBy = null;
            if (entity.CreatedAuthor != null)
                createdBy = new AuditAuthor<TKey>(entity.CreatedOn, entity.CreatedAuthor.Id, entity.CreatedAuthor.Name);
            if (entity.ChangedAuthor != null)
                changedBy = new AuditAuthor<TKey>(entity.ChangedOn.Value, entity.ChangedAuthor.Id.Value, entity.ChangedAuthor.Name);
            dto.CreatedBy = createdBy;
            dto.ChangedBy = changedBy;

        }

    }
}

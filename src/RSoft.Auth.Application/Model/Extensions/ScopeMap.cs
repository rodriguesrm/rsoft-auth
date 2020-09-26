using RSoft.Auth.Domain.Entities;

namespace RSoft.Auth.Application.Model.Extensions
{

    /// <summary>
    /// Contains methods for Scope entity-dto mapping
    /// </summary>
    public static class ScopeMap
    {

        /// <summary>
        /// Map dto to entity
        /// </summary>
        /// <param name="dto">Object to extension</param>
        public static Scope Map(this ScopeDto dto)
            => Map(dto, false);

        /// <summary>
        /// Map dto to entity
        /// </summary>
        /// <param name="dto">Object to extension</param>
        /// <param name="setId">Set dto id into entity</param>
        public static Scope Map(this ScopeDto dto, bool setId)
        {

            Scope entity;

            if (setId)
                entity = new Scope(dto.Id);
            else
                entity = new Scope();

            entity.Name = dto.Name;
            entity.AccessKey = dto.AccessKey;
            entity.IsActive = dto.IsActive;

            return entity;
        }

        /// <summary>
        /// Map entity to dto
        /// </summary>
        /// <param name="entity">Object to extension</param>
        /// <param name="dto">Scope Dto object</param>
        public static Scope Map(this Scope entity, ScopeDto dto)
        {
            if (dto != null)
            {
                entity.Name = dto.Name;
                entity.AccessKey = dto.AccessKey;
                entity.IsActive = dto.IsActive;
            }
            return entity;
        }

        /// <summary>
        /// Map entity to dto
        /// </summary>
        /// <param name="Scope">Object to extension</param>
        public static ScopeDto Map(this Scope Scope)
            => Map(Scope, true);

        /// <summary>
        /// Map entity to dto
        /// </summary>
        /// <param name="entity">Object to extension</param>
        /// <param name="addAuthors">Indicate if add authors data in dto</param>
        public static ScopeDto Map(this Scope entity, bool addAuthors)
        {

            if (entity == null)
                return null;
            
            ScopeDto dto = new ScopeDto();

            if (entity.Valid)
            {

                dto.Id = entity.Id;
                if (addAuthors)
                    Author.Map(dto, entity);

            }
            else
            {
                dto.AddNotifications(entity.Notifications);
            }

            dto.Name = entity.Name;
            dto.AccessKey = entity.AccessKey;
            dto.IsActive = entity.IsActive;

            return dto;

        }

    }

}

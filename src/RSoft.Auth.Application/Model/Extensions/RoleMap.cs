using RSoft.Auth.Domain.Entities;

namespace RSoft.Auth.Application.Model.Extensions
{

    /// <summary>
    /// Contains methods for role entity-dto mapping
    /// </summary>
    public static class RoleMap
    {

        /// <summary>
        /// Map dto to entity
        /// </summary>
        /// <param name="dto">Object to extension</param>
        public static Role Map(this RoleDto dto)
        {
            return new Role()
            {
                Name = dto.Name,
                Description = dto.Description,
                IsActive = dto.IsActive
            };
        }

        /// <summary>
        /// Map entity to dto
        /// </summary>
        /// <param name="entity">Object to extension</param>
        /// <param name="dto">Role Dto object</param>
        public static Role Map(this Role entity, RoleDto dto)
        {
            if (dto != null)
            {
                entity.Name = dto.Name;
                entity.Description = dto.Description;
                entity.IsActive = dto.IsActive;
            }
            return entity;
        }

        /// <summary>
        /// Map entity to dto
        /// </summary>
        /// <param name="Role">Object to extension</param>
        public static RoleDto Map(this Role Role)
            => Map(Role, true);

        /// <summary>
        /// Map entity to dto
        /// </summary>
        /// <param name="entity">Object to extension</param>
        /// <param name="addAuthors">Indicate if add authors data in dto</param>
        public static RoleDto Map(this Role entity, bool addAuthors)
        {
            
            RoleDto dto = new RoleDto();

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
            dto.Description = entity.Description;
            dto.IsActive = entity.IsActive;

            return dto;

        }

    }

}

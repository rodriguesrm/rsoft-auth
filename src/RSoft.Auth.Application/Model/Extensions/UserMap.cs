using RSoft.Auth.Domain.Entities;
using RSoft.Framework.Domain.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace RSoft.Auth.Application.Model.Extensions
{

    /// <summary>
    /// Contains methods for user entity-dto mapping
    /// </summary>
    public static class UserMap
    {

        /// <summary>
        /// Map children data from entity to dto
        /// </summary>
        /// <param name="entity">Entity object</param>
        /// <param name="dto">Dto object</param>
        private static void LoadChildren(User entity, UserDto dto)
        {
            ICollection<RoleDto> roles = null;
            ICollection<ScopeDto> scopes = null;
            if (entity.Roles?.Count > 0)
                roles = entity.Roles.Select(r => r.Map()).ToList();
            if (entity.Scopes?.Count > 0)
                scopes = entity.Scopes.Select(s => s.Map()).ToList();

            dto.Roles = roles;
            dto.Scopes = scopes;
        }

        /// <summary>
        /// Map dto to entity
        /// </summary>
        /// <param name="dto">Object to extension</param>
        public static User Map(this UserDto dto)
        {
            ICollection<Role> roles = dto.Roles?.Select(r => r.Map()).ToList();
            ICollection<Scope> scopes = dto.Scopes?.Select(s => s.Map()).ToList();
            return new User()
            {
                Name = new Name( dto.FirstName, dto.LastName),
                BornDate = dto.BornDate,
                Email = new Email(dto.Email),
                IsActive = dto.IsActive,
                Roles = roles,
                Scopes = scopes
            };
        }

        /// <summary>
        /// Map entity to dto
        /// </summary>
        /// <param name="entity">Object to extension</param>
        /// <param name="dto">User Dto object</param>
        public static User Map(this User entity, UserDto dto)
            => Map(entity, dto, true);

        /// <summary>
        /// Map entity to dto
        /// </summary>
        /// <param name="entity">Object to extension</param>
        /// <param name="dto">User Dto object</param>
        /// <param name="loadChildren">Indicate if load children data in dto</param>
        public static User Map(this User entity, UserDto dto, bool loadChildren)
        {
            if (dto != null)
            {
                entity.Name = new Name(dto.FirstName, dto.LastName);
                entity.BornDate = dto.BornDate;
                entity.Email = new Email(dto.Email);
                entity.IsActive = dto.IsActive;
                if (loadChildren)
                    LoadChildren(entity, dto);
            }
            return entity;
        }

        /// <summary>
        /// Map entity to dto
        /// </summary>
        /// <param name="entity">Object to extension</param>
        public static UserDto Map(this User entity)
            => Map(entity, true, true);

        /// <summary>
        /// Map entity to dto
        /// </summary>
        /// <param name="entity">Object to extension</param>
        /// <param name="addAuthors">Indicate if add authors data in dto</param>
        /// <param name="loadChildren">Indicate if load children data in dto</param>
        public static UserDto Map(this User entity, bool addAuthors, bool loadChildren)
        {
            
            UserDto dto = new UserDto();

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

            dto.FirstName = entity.Name.FirstName;
            dto.LastName = entity.Name.LastName;
            dto.BornDate = entity.BornDate;
            dto.Email = entity.Email.Address;
            dto.IsActive = entity.IsActive;

            if (loadChildren)
                LoadChildren(entity, dto);

            return dto;

        }

    }

}

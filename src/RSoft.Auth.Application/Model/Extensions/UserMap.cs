﻿using RSoft.Auth.Domain.Entities;

namespace RSoft.Auth.Application.Model.Extensions
{

    /// <summary>
    /// Contains methods for user entity-dto mapping
    /// </summary>
    public static class UserMap
    {

        /// <summary>
        /// Map dto to entity
        /// </summary>
        /// <param name="dto">Object to extension</param>
        public static User Map(this UserDto dto)
        {
            return new User()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                BornDate = dto.BornDate,
                Email = dto.Email,
                IsActive = dto.IsActive
            };
        }

        /// <summary>
        /// Map entity to dto
        /// </summary>
        /// <param name="entity">Object to extension</param>
        /// <param name="dto">User Dto object</param>
        /// <returns></returns>
        public static User Map(this User entity, UserDto dto)
        {
            if (dto != null)
            {
                entity.FirstName = dto.FirstName;
                entity.LastName = dto.LastName;
                entity.BornDate = dto.BornDate;
                entity.Email = dto.Email;
                entity.IsActive = dto.IsActive;
            }
            return entity;
        }

        /// <summary>
        /// Map entity to dto
        /// </summary>
        /// <param name="entity">Object to extension</param>
        public static UserDto Map(this User entity)
            => Map(entity, true);

        /// <summary>
        /// Map entity to dto
        /// </summary>
        /// <param name="entity">Object to extension</param>
        /// <param name="addAuthors">Indicate if add authors data in dto</param>
        public static UserDto Map(this User entity, bool addAuthors)
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

            dto.FirstName = entity.FirstName;
            dto.LastName = entity.LastName;
            dto.BornDate = entity.BornDate;
            dto.Email = entity.Email;
            dto.IsActive = entity.IsActive;

            return dto;

        }

    }

}

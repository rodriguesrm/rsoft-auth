﻿using RSoft.Auth.Domain.Entities;
using RSoft.Lib.Common.ValueObjects;
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
        /// Map related data from entity to dto
        /// </summary>
        /// <param name="entity">Entity object</param>
        /// <param name="dto">Dto object</param>
        private static void LoadLazy(User entity, UserDto dto)
        {
            ICollection<RoleDto> roles = null;
            ICollection<AppClientDto> appClients = null;
            if (entity.Roles?.Count > 0)
                roles = entity.Roles.Select(r => r.Map()).ToList();
            if (entity.ApplicationClients?.Count > 0)
                appClients = entity.ApplicationClients.Select(s => s.Map()).ToList();

            dto.Roles = roles;
            dto.ApplicationClients = appClients;
            if (entity.Credential != null)
                dto.Credential = entity.Credential.Map();
        }

        /// <summary>
        /// Map dto to entity
        /// </summary>
        /// <param name="dto">Object to extension</param>
        public static User Map(this UserDto dto)
        {
            ICollection<Role> roles = dto.Roles?.Select(r => r.Map(true)).ToList();
            ICollection<AppClient> appClients = dto.ApplicationClients?.Select(s => s.Map(true)).ToList();
            return new User()
            {
                Document = dto.Document,
                Name = new Name(dto.Name?.FirstName, dto.Name?.LastName),
                BornDate = dto.BornDate,
                Email = new Email(dto.Email),
                Type = dto.Type,
                IsActive = dto.IsActive,
                Roles = roles,
                ApplicationClients = appClients
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
        /// <param name="useLazy">Indicate if load related data in dto</param>
        public static User Map(this User entity, UserDto dto, bool useLazy)
        {
            if (dto != null)
            {
                entity.Document = dto.Document;
                entity.Name = new Name(dto.Name?.FirstName, dto.Name?.LastName);
                entity.BornDate = dto.BornDate;
                entity.Email = new Email(dto.Email);
                entity.Type = dto.Type;
                entity.IsActive = dto.IsActive;
                if (useLazy)
                    LoadLazy(entity, dto);
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
        /// <param name="useLazy">Indicate if load related data in dto</param>
        public static UserDto Map(this User entity, bool addAuthors, bool useLazy)
        {

            if (entity == null)
                return null;
            
            UserDto dto = new UserDto();

            if (entity.Valid)
            {

                dto.Id = entity.Id;
                if (addAuthors)
                    AuthorMap.Map(dto, entity);

            }
            else
            {
                dto.AddNotifications(entity.Notifications);
            }

            dto.Document = entity.Document;
            dto.Name = entity.Name;
            dto.BornDate = entity.BornDate;
            dto.Email = entity.Email.Address;
            dto.IsActive = entity.IsActive;
            dto.Type = entity.Type;

            if (useLazy)
                LoadLazy(entity, dto);

            return dto;

        }

    }

}

﻿using RSoft.Auth.Domain.Entities;
using RSoft.Lib.Common.Models;
using System;

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
            => Map(dto, false);

        /// <summary>
        /// Map dto to entity
        /// </summary>
        /// <param name="dto">Object to extension</param>
        /// <param name="setId">Set dto id into entity</param>
        public static Role Map(this RoleDto dto, bool setId)
        {

            Role entity;

            if (setId)
                entity = new Role(dto.Id);
            else
                entity = new Role();
            
            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.IsActive = dto.IsActive;
            entity.AppClient = (dto.AppClient?.Id) != null ? new AppClient(id: dto.AppClient.Id.Value) : null;

            return entity;

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
                entity.AppClient = dto.AppClient?.Id != null ? new AppClient(dto.AppClient.Id.Value) : null;
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

            if (entity == null)
                return null;

            RoleDto dto = new RoleDto();

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

            dto.Name = entity.Name;
            dto.Description = entity.Description;
            dto.AppClient = new SimpleIdentification<Guid>(entity.AppClient?.Id, entity.AppClient?.Name);
            dto.IsActive = entity.IsActive;

            return dto;

        }

    }

}

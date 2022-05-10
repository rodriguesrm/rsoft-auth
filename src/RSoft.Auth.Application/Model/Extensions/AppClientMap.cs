using RSoft.Auth.Domain.Entities;
using System;

namespace RSoft.Auth.Application.Model.Extensions
{

    /// <summary>
    /// Contains methods for App-Client entity-dto mapping
    /// </summary>
    public static class AppClientMap
    {

        /// <summary>
        /// Map dto to entity
        /// </summary>
        /// <param name="dto">Object to extension</param>
        public static AppClient Map(this AppClientDto dto)
            => Map(dto, false);

        /// <summary>
        /// Map dto to entity
        /// </summary>
        /// <param name="dto">Object to extension</param>
        /// <param name="setId">Set dto id into entity</param>
        public static AppClient Map(this AppClientDto dto, bool setId)
        {

            AppClient entity;

            if (setId)
                entity = new AppClient(dto.Id);
            else
                entity = new AppClient();

            entity.Name = dto.Name;
            entity.AccessKey = dto.AccessKey;
            entity.AllowLogin = dto.AllowLogin;
            entity.IsActive = dto.IsActive;

            return entity;
        }

        /// <summary>
        /// Map entity to dto
        /// </summary>
        /// <param name="entity">Object to extension</param>
        /// <param name="dto">AppClient Dto object</param>
        public static AppClient Map(this AppClient entity, AppClientDto dto)
        {
            if (dto != null)
            {
                entity.Name = dto.Name;
                entity.AccessKey = dto.AccessKey != Guid.Empty ? dto.AccessKey : entity.AccessKey;
                entity.AllowLogin = dto.AllowLogin;
                entity.IsActive = dto.IsActive;
            }
            return entity;
        }

        /// <summary>
        /// Map entity to dto
        /// </summary>
        /// <param name="appClient">Object to extension</param>
        public static AppClientDto Map(this AppClient appClient)
            => Map(appClient, true);

        /// <summary>
        /// Map entity to dto
        /// </summary>
        /// <param name="entity">Object to extension</param>
        /// <param name="addAuthors">Indicate if add authors data in dto</param>
        public static AppClientDto Map(this AppClient entity, bool addAuthors)
        {

            if (entity == null)
                return null;
            
            AppClientDto dto = new();

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
            dto.AccessKey = entity.AccessKey;
            dto.AllowLogin = entity.AllowLogin;
            dto.IsActive = entity.IsActive;

            return dto;

        }

    }

}

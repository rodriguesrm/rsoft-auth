using RSoft.Framework.Application.Model;
using RSofth.Auth.Domain.Entities;
using System;

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
        /// <param name="user">Object to extension</param>
        public static UserDto Map(this User user)
            => Map(user, true);

        /// <summary>
        /// Map entity to dto
        /// </summary>
        /// <param name="user">Object to extension</param>
        /// <param name="addAuthors">Indicate if add authors data in dto</param>
        public static UserDto Map(this User user, bool addAuthors)
        {
            
            UserDto dto = new UserDto();

            if (user.Valid)
            {

                dto.Id = user.Id;

                if (addAuthors)
                {

                    //TODO: Create a generic method for this
                    AuditAuthor<Guid> createdBy = new AuditAuthor<Guid>(user.CreatedOn, user.CreatedBy, user.CreatedAuthor?.GetFullName());
                    AuditAuthor<Guid> changedBy = null;
                    if (user.ChangedAuthor != null)
                        changedBy = new AuditAuthor<Guid>(user.ChangedOn.Value, user.ChangedBy.Value, user.ChangedAuthor?.GetFullName());
                    dto.CreatedBy = createdBy;
                    dto.ChangedBy = changedBy;
                }

            }
            else
            {
                dto.AddNotifications(user.Notifications);
            }

            dto.FirstName = user.FirstName;
            dto.LastName = user.LastName;
            dto.BornDate = user.BornDate;
            dto.Email = user.Email;
            dto.IsActive = user.IsActive;

            return dto;

        }

    }

}

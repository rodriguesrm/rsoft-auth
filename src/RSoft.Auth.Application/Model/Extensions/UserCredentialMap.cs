using RSoft.Auth.Domain.Entities;

namespace RSoft.Auth.Application.Model.Extensions
{

    /// <summary>
    /// Provides methods to map user credential data between entity <-> dto
    /// </summary>
    public static class UserCredentialMap
    {

        /// <summary>
        /// Map dto to entity
        /// </summary>
        /// <param name="dto">User credential dto object instance</param>
        public static UserCredential Map(this UserCredentialDto dto)
        {
            return new UserCredential()
            {
                UserId = dto.UserId,
                Login = dto.Login,
                Password = dto.Password,
                ChangeCredentials = dto.ChangeCredentials
            };
        }

        /// <summary>
        /// Map entity to dto
        /// </summary>
        /// <param name="entity">Object to extension</param>
        /// <param name="dto">User Dto object</param>
        public static UserCredential Map(this UserCredential entity, UserCredentialDto dto)
        {
            if (dto != null)
            {
                entity.UserId = dto.UserId;
                entity.Login = dto.Login;
                //Password => NEVER MAP THIS FIELD HERE
                entity.ChangeCredentials = dto.ChangeCredentials;
            }
            return entity;
        }

        public static UserCredentialDto Map(this UserCredential entity)
        {

            if (entity == null)
                return null;

            UserCredentialDto dto = new UserCredentialDto();

            if (!entity.Valid)
                dto.AddNotifications(entity.Notifications);

            dto.UserId = entity.UserId;
            dto.Login = entity.Login;
            //Password => NEVER MAP THIS FIELD HERE
            dto.ChangeCredentials = entity.ChangeCredentials;

            return dto;

        }

    }

}

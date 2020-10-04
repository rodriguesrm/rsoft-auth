using UserCredentialTokenDomain = RSoft.Auth.Domain.Entities.UserCredentialToken;
using RSoft.Auth.Infra.Data.Entities;

namespace RSoft.Auth.Infra.Data.Extensions
{

    /// <summary>
    /// UserCredentialToken extensions
    /// </summary>
    public static class UserCredentialTokenExtension
    {


        /// <summary>
        /// Maps table to entity
        /// </summary>
        /// <param name="table">Table entity to map</param>
        public static UserCredentialTokenDomain Map(this UserCredentialToken table)
            => Map(table, true);

        /// <summary>
        /// Maps table to entity
        /// </summary>
        /// <param name="table">Table entity to map</param>
        /// <param name="useLazy">Load related data data</param>
        public static UserCredentialTokenDomain Map(this UserCredentialToken table, bool useLazy)
        {
            UserCredentialTokenDomain result = null;

            if (table != null)
            {

                result = new UserCredentialTokenDomain(table.Id)
                {
                    UserId = table.UserId,
                    CreatedAt = table.CreatedAt,
                    ExpiresOn = table.ExpiresOn,
                    FirstAccess =table.FirstAccess
                };

                if (useLazy)
                {
                    result.User = table.User?.Map(false);
                }

            }

            return result;

        }

        /// <summary>
        /// Maps entity to table
        /// </summary>
        /// <param name="entity">Domain entity to map</param>
        public static UserCredentialToken Map(this UserCredentialTokenDomain entity)
        {

            UserCredentialToken result = null;

            if (entity != null)
            {
                result = new UserCredentialToken(entity.Id)
                {
                    UserId = entity.UserId.Value,
                    CreatedAt = entity.CreatedAt,
                    ExpiresOn = entity.ExpiresOn,
                    FirstAccess = entity.FirstAccess
                };
            }

            return result;

        }

        /// <summary>
        /// Maps entity to an existing table
        /// </summary>
        /// <param name="entity">Domain entity to map</param>
        /// <param name="table">Instance of existing table entity</param>
        public static UserCredentialToken Map(this UserCredentialTokenDomain entity, UserCredentialToken table)
        {

            if (entity != null && table != null)
            {
                table.UserId = entity.UserId.Value;
                table.CreatedAt = entity.CreatedAt;
                table.ExpiresOn = entity.ExpiresOn;
                table.FirstAccess = entity.FirstAccess;
            }

            return table;

        }

    }

}

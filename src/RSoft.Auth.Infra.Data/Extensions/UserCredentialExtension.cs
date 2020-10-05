using UserCredentialDomain = RSoft.Auth.Domain.Entities.UserCredential;
using RSoft.Auth.Infra.Data.Entities;

namespace RSoft.Auth.Infra.Data.Extensions
{

    /// <summary>
    /// UserCredential extensions
    /// </summary>
    public static class UserCredentialExtension
    {

        /// <summary>
        /// Maps table to entity
        /// </summary>
        /// <param name="table">Table entity to map</param>
        public static UserCredentialDomain Map(this UserCredential table)
            => Map(table, true);

        /// <summary>
        /// Maps table to entity
        /// </summary>
        /// <param name="table">Table entity to map</param>
        /// <param name="useLazy">Indicate load related data flag</param>
        public static UserCredentialDomain Map(this UserCredential table, bool useLazy)
        {
            UserCredentialDomain result = null;

            if (table != null)
            {

                result = new UserCredentialDomain()
                {
                    UserId = table.UserId,
                    Login = table.Login,
                    Password = table.Password,
                    ChangeCredentials = table.ChangeCredentials,
                    AuthFailsQty = table.AuthFailsQty,
                    LockoutUntil = table.LockoutUntil
                };

            }

            return result;

        }

        /// <summary>
        /// Maps entity to table
        /// </summary>
        /// <param name="entity">Domain entity to map</param>
        public static UserCredential Map(this UserCredentialDomain entity)
        {

            UserCredential result = null;

            if (entity != null)
            {
                result = new UserCredential()
                {
                    UserId = entity.UserId,
                    Login = entity.Login,
                    Password = entity.Password,
                    ChangeCredentials = entity.ChangeCredentials,
                    AuthFailsQty = entity.AuthFailsQty,
                    LockoutUntil = entity.LockoutUntil
                };
            }

            return result;

        }

        /// <summary>
        /// Maps entity to an existing table
        /// </summary>
        /// <param name="entity">Domain entity to map</param>
        /// <param name="table">Instance of existing table entity</param>
        public static UserCredential Map(this UserCredentialDomain entity, UserCredential table)
        {

            if (entity != null && table != null)
            {
                table.UserId = entity.UserId;
                table.Login = entity.Login;
                table.Password = entity.Password;
                table.ChangeCredentials = entity.ChangeCredentials;
                table.AuthFailsQty = entity.AuthFailsQty;
                table.LockoutUntil = entity.LockoutUntil;
            }

            return table;

        }

    }

}

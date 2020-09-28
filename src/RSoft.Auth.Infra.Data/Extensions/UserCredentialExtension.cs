using dmn = RSoft.Auth.Domain.Entities;
using tbl = RSoft.Auth.Infra.Data.Entities;

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
        public static dmn.UserCredential Map(this tbl.UserCredential table)
            => Map(table, true);

        /// <summary>
        /// Maps table to entity
        /// </summary>
        /// <param name="table">Table entity to map</param>
        /// <param name="loadChildren">Indicate load children data flag</param>
        public static dmn.UserCredential Map(this tbl.UserCredential table, bool loadChildren)
        {
            dmn.UserCredential result = null;

            if (table != null)
            {

                result = new dmn.UserCredential()
                {
                    UserId = table.UserId,
                    Login = table.Login,
                    Key = table.Key,
                    Password = table.Password,
                    ChangeCredentials = table.ChangeCredentials
                };

            }

            return result;

        }

        /// <summary>
        /// Maps entity to table
        /// </summary>
        /// <param name="entity">Domain entity to map</param>
        public static tbl.UserCredential Map(this dmn.UserCredential entity)
        {

            tbl.UserCredential result = null;

            if (entity != null)
            {
                result = new tbl.UserCredential()
                {
                    UserId = entity.UserId,
                    Login = entity.Login,
                    Key = entity.Key,
                    Password = entity.Password,
                    ChangeCredentials = entity.ChangeCredentials
                };
            }

            return result;

        }

        /// <summary>
        /// Maps entity to an existing table
        /// </summary>
        /// <param name="entity">Domain entity to map</param>
        /// <param name="table">Instance of existing table entity</param>
        public static tbl.UserCredential Map(this dmn.UserCredential entity, tbl.UserCredential table)
        {

            if (entity != null && table != null)
            {
                table.UserId = entity.UserId;
                table.Login = entity.Login;
                table.Key = entity.Key;
                table.Password = entity.Password;
                table.ChangeCredentials = entity.ChangeCredentials;
            }

            return table;

        }

    }

}

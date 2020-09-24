using dmn = RSoft.Auth.Domain.Entities;
using tbl = RSoft.Auth.Infra.Data.Entities;

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
        public static dmn.UserCredentialToken Map(this tbl.UserCredentialToken table)
            => Map(table, true);

        /// <summary>
        /// Maps table to entity
        /// </summary>
        /// <param name="table">Table entity to map</param>
        /// <param name="loadChildren">Load children data</param>
        public static dmn.UserCredentialToken Map(this tbl.UserCredentialToken table, bool loadChildren)
        {
            dmn.UserCredentialToken result = null;

            if (table != null)
            {

                result = new dmn.UserCredentialToken(table.Id)
                {
                    UserId = table.UserId,
                    CreatedAt = table.CreatedAt,
                    ExpiresOn = table.ExpiresOn,
                    FirstAccess =table.FirstAccess
                };

                if (loadChildren)
                {
                    result.User = table.User?.Map(false);
                }

                result.Validate();

            }

            return result;

        }

        /// <summary>
        /// Maps entity to table
        /// </summary>
        /// <param name="entity">Domain entity to map</param>
        public static tbl.UserCredentialToken Map(this dmn.UserCredentialToken entity)
        {

            tbl.UserCredentialToken result = null;

            if (entity != null)
            {
                result = new tbl.UserCredentialToken(entity.Id)
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
        public static tbl.UserCredentialToken Map(this dmn.UserCredentialToken entity, tbl.UserCredentialToken table)
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

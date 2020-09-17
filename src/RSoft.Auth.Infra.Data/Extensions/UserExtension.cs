using dmn = RSoft.Auth.Domain.Entities;
using tbl = RSoft.Auth.Infra.Data.Entities;

namespace RSoft.Auth.Infra.Data.Extensions
{

    /// <summary>
    /// User extensions
    /// </summary>
    public static class UserExtension
    {

        /// <summary>
        /// Maps table to entity
        /// </summary>
        /// <param name="table">Table entity to map</param>
        public static dmn.User Map(this tbl.User table)
            => Map(table, true);

        /// <summary>
        /// Maps table to entity
        /// </summary>
        /// <param name="table">Table entity to map</param>
        /// <param name="loadChildren">Load children data</param>
        public static dmn.User Map(this tbl.User table, bool loadChildren)
        {

            dmn.User result = new dmn.User(table.Id)
            {
                FirstName = table.FirstName,
                LastName = table.LastName,
                BornDate = table.BornDate,
                Email = table.Email,
                CreatedOn = table.CreatedOn,
                ChangedOn = table.ChangedOn
            };

            if (loadChildren)
            {
                result.MapAuthor(table);
                if (table.Credential != null)
                {
                    result.Credential = new dmn.UserCredential()
                    {
                        UserKey = table.Credential.UserKey,
                        Password = table.Credential.Password
                    };
                }
            }

            result.Validate();

            return result;

        }

        /// <summary>
        /// Maps entity to table
        /// </summary>
        /// <param name="entity">Domain entity to map</param>
        public static tbl.User Map(this dmn.User entity)
        {

            tbl.User result = new tbl.User(entity.Id)
            {
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                BornDate = entity.BornDate,
                Email = entity.Email,
                CreatedOn = entity.CreatedOn,
                CreatedBy = entity.CreatedAuthor.Id
            };

            return result;

        }

        /// <summary>
        /// Maps entity to an existing table
        /// </summary>
        /// <param name="entity">Domain entity to map</param>
        /// <param name="table">Instance of existing table entity</param>
        public static tbl.User Map(this dmn.User entity, tbl.User table)
        {

            table.FirstName = entity.FirstName;
            table.LastName = entity.LastName;
            table.BornDate = entity.BornDate;
            table.Email = entity.Email;
            table.ChangedOn = entity.ChangedOn;
            table.ChangedBy = entity.ChangedAuthor.Id;

            return table;

        }

    }

}

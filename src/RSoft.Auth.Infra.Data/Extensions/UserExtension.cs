using RSoft.Framework.Domain.ValueObjects;
using System.Linq;
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

            dmn.User result = null;
            if (table != null)
            {

                result = new dmn.User(table.Id)
                {
                    Name = new Name(table.FirstName, table.LastName),
                    BornDate = table.BornDate,
                    Email = new Email(table.Email),
                    Type = table.Type,
                    CreatedOn = table.CreatedOn,
                    ChangedOn = table.ChangedOn
                };

                if (loadChildren)
                {
                    result.MapAuthor(table);
                    if (table.Credential != null)
                    {
                        result.Credential = table.Credential.Map();
                    }
                    result.Roles = table.Roles.Select(r => r.Role.Map(false)).ToList();
                    result.Scopes = table.Scopes.Select(s => s.Scope.Map(false)).ToList();
                }

                //result.Validate();

            }

            return result;

        }

        /// <summary>
        /// Maps entity to table
        /// </summary>
        /// <param name="entity">Domain entity to map</param>
        public static tbl.User Map(this dmn.User entity)
            => Map(entity, true);

        /// <summary>
        /// Maps entity to table
        /// </summary>
        /// <param name="entity">Domain entity to map</param>
        /// <param name="loadChildren">Load children data</param>
        public static tbl.User Map(this dmn.User entity, bool loadChildren)
        {

            tbl.User result = null;

            if (entity != null)
            {

                result = new tbl.User(entity.Id)
                {
                    FirstName = entity.Name.FirstName,
                    LastName = entity.Name.LastName,
                    BornDate = entity.BornDate,
                    Email = entity.Email?.Address,
                    Type = entity.Type.Value,
                    CreatedOn = entity.CreatedOn,
                    CreatedBy = entity.CreatedAuthor.Id
                };

                if (loadChildren)
                {
                    result.Scopes = entity.Scopes.Select(s => new tbl.UserScope() { ScopeId = s.Id, UserId = entity.Id }).ToList();
                    result.Roles = entity.Roles.Select(r => new tbl.UserRole() { RoleId = r.Id, UserId = entity.Id }).ToList();
                }


            }

            return result;

        }

        /// <summary>
        /// Maps entity to an existing table
        /// </summary>
        /// <param name="entity">Domain entity to map</param>
        /// <param name="table">Instance of existing table entity</param>
        public static tbl.User Map(this dmn.User entity, tbl.User table)
        {

            if (entity != null && table != null)
            {

                table.FirstName = entity.Name.FirstName;
                table.LastName = entity.Name.LastName;
                table.BornDate = entity.BornDate;
                table.Email = entity.Email.Address;
                table.Type = entity.Type.Value;
                table.ChangedOn = entity.ChangedOn;
                table.ChangedBy = entity.ChangedAuthor.Id;

            }

            return table;

        }

    }

}

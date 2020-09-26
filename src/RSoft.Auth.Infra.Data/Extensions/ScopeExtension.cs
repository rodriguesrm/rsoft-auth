using System.Linq;
using dmn = RSoft.Auth.Domain.Entities;
using tbl = RSoft.Auth.Infra.Data.Entities;

namespace RSoft.Auth.Infra.Data.Extensions
{

    /// <summary>
    /// Scope extensions
    /// </summary>
    public static class ScopeExtension
    {

        /// <summary>
        /// Maps table to entity
        /// </summary>
        /// <param name="table">Table entity to map</param>
        public static dmn.Scope Map(this tbl.Scope table)
            => Map(table, true);

        /// <summary>
        /// Maps table to entity
        /// </summary>
        /// <param name="table">Table entity to map</param>
        /// <param name="loadChildren">Load children data</param>
        public static dmn.Scope Map(this tbl.Scope table, bool loadChildren)
        {

            dmn.Scope result = null;

            if (table != null)
            {

                result = new dmn.Scope(table.Id)
                {
                    Name = table.Name,
                    CreatedOn = table.CreatedOn,
                    ChangedOn = table.ChangedOn,
                    IsActive = table.IsActive
                };

                if (loadChildren)
                {
                    result.MapAuthor(table);
                    if (table.Users?.Count > 0)
                        result.Users = table.Users.Select(u => u.User.Map(false)).ToList();
                    if (table.Roles?.Count > 0)
                        result.Roles = table.Roles.Select(r => r.Map(false)).ToList();
                }

                //result.Validate();

            }

            return result;

        }

        /// <summary>
        /// Maps entity to table
        /// </summary>
        /// <param name="entity">Domain entity to map</param>
        public static tbl.Scope Map(this dmn.Scope entity)
        {

            tbl.Scope result = null;

            if (entity != null)
            {
                result = new tbl.Scope(entity.Id)
                {
                    Name = entity.Name,
                    CreatedOn = entity.CreatedOn,
                    CreatedBy = entity.CreatedAuthor.Id,
                    IsActive = entity.IsActive
                };
            }

            return result;

        }

        /// <summary>
        /// Maps entity to an existing table
        /// </summary>
        /// <param name="entity">Domain entity to map</param>
        /// <param name="table">Instance of existing table entity</param>
        public static tbl.Scope Map(this dmn.Scope entity, tbl.Scope table)
        {

            if (entity != null && table != null)
            {
                table.Name = entity.Name;
                table.ChangedOn = entity.ChangedOn;
                table.ChangedBy = entity.ChangedAuthor.Id;
                table.IsActive = entity.IsActive;
            }

            return table;

        }

    }

}

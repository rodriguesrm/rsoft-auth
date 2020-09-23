using System.Linq;
using dmn = RSoft.Auth.Domain.Entities;
using tbl = RSoft.Auth.Infra.Data.Entities;

namespace RSoft.Auth.Infra.Data.Extensions
{

    /// <summary>
    /// Role extensions
    /// </summary>
    public static class RoleExtension
    {


        /// <summary>
        /// Maps table to entity
        /// </summary>
        /// <param name="table">Table entity to map</param>
        public static dmn.Role Map(this tbl.Role table)
            => Map(table, true);

        /// <summary>
        /// Maps table to entity
        /// </summary>
        /// <param name="table">Table entity to map</param>
        /// <param name="loadChildren">Load children data</param>
        public static dmn.Role Map(this tbl.Role table, bool loadChildren)
        {
            dmn.Role result = null;

            if (table != null)
            {

                result = new dmn.Role(table.Id)
                {
                    Name = table.Name,
                    Description = table.Description,
                    CreatedOn = table.CreatedOn,
                    ChangedOn = table.ChangedOn,
                    IsActive = table.IsActive
                };

                if (loadChildren)
                {
                    result.MapAuthor(table);
                    if (table.Users?.Count > 0)
                        result.Users = table.Users.Select(u => u.User.Map(false)).ToList();
                    result.Scope = table.Scope?.Map(false);
                }

                result.Validate();

            }

            return result;

        }

        /// <summary>
        /// Maps entity to table
        /// </summary>
        /// <param name="entity">Domain entity to map</param>
        public static tbl.Role Map(this dmn.Role entity)
        {

            tbl.Role result = null;

            if (entity != null)
            {
                result = new tbl.Role(entity.Id)
                {
                    Name = entity.Name,
                    Description = entity.Description,
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
        public static tbl.Role Map(this dmn.Role entity, tbl.Role table)
        {

            if (entity != null && table != null)
            {
                table.Name = entity.Name;
                table.Description = entity.Description;
                table.ChangedOn = entity.ChangedOn;
                table.ChangedBy = entity.ChangedAuthor.Id;
                table.IsActive = entity.IsActive;
            }

            return table;

        }

    }

}

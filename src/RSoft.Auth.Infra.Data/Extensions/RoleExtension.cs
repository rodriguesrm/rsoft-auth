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
        /// Map table to entity
        /// </summary>
        /// <param name="table"></param>
        public static dmn.Role Map(this tbl.Role table)
            => Map(table, true);

        /// <summary>
        /// Map table to entity
        /// </summary>
        /// <param name="table"></param>
        /// <param name="loadChildren">Load children data</param>
        public static dmn.Role Map(this tbl.Role table, bool loadChildren)
        {
            dmn.Role result = new dmn.Role(table.Id)
            {
                Name = table.Name,
                Description = table.Description,
                CreatedOn = table.CreatedOn,
                ChangedOn = table.ChangedOn
            };

            if (loadChildren)
            {
                result.MapAuthor(table);
                //TODO: RR - Map users with loadChildren=false for prevent cyclical calls
                //TODO: RR - Map scopes with loadChildren=false for prevent cyclical calls
            }

            result.Validate();

            return result;

        }

    }

}

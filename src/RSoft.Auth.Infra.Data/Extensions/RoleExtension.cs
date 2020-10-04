using System.Linq;
using ScopeDomain = RSoft.Auth.Domain.Entities.Scope;
using RoleDomain = RSoft.Auth.Domain.Entities.Role;
using RSoft.Auth.Infra.Data.Entities;

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
        public static RoleDomain Map(this Role table)
            => Map(table, true);

        /// <summary>
        /// Maps table to entity
        /// </summary>
        /// <param name="table">Table entity to map</param>
        /// <param name="useLazy">Load related data</param>
        public static RoleDomain Map(this Role table, bool useLazy)
        {
            RoleDomain result = null;

            if (table != null)
            {

                result = new RoleDomain(table.Id)
                {
                    Name = table.Name,
                    Description = table.Description,
                    CreatedOn = table.CreatedOn,
                    ChangedOn = table.ChangedOn,
                    IsActive = table.IsActive
                };

                if (useLazy)
                {
                    result.MapAuthor(table);
                    if (table.Users?.Count > 0)
                        result.Users = table.Users.Select(u => u.User.Map(false)).ToList();
                    result.Scope = table.Scope?.Map(false);
                }
                else
                {
                    result.Scope = new ScopeDomain(table.ScopeId);
                }

                //result.Validate();

            }

            return result;

        }

        /// <summary>
        /// Maps entity to table
        /// </summary>
        /// <param name="entity">Domain entity to map</param>
        public static Role Map(this RoleDomain entity)
        {

            Role result = null;

            if (entity != null)
            {
                result = new Role(entity.Id)
                {
                    ScopeId = entity.Scope.Id,
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
        public static Role Map(this RoleDomain entity, Role table)
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

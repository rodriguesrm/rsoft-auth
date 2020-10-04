﻿using System.Linq;
using ScopeDomain = RSoft.Auth.Domain.Entities.Scope;
using RSoft.Auth.Infra.Data.Entities;

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
        public static ScopeDomain Map(this Scope table)
            => Map(table, true);

        /// <summary>
        /// Maps table to entity
        /// </summary>
        /// <param name="table">Table entity to map</param>
        /// <param name="useLazy">Load related data</param>
        public static ScopeDomain Map(this Scope table, bool useLazy)
        {

            ScopeDomain result = null;

            if (table != null)
            {

                result = new ScopeDomain(table.Id)
                {
                    Name = table.Name,
                    AccessKey = table.AccessKey,
                    CreatedOn = table.CreatedOn,
                    ChangedOn = table.ChangedOn,
                    IsActive = table.IsActive
                };

                if (useLazy)
                {
                    result.MapAuthor(table);
                    if (table.Users?.Count > 0)
                        result.Users = table.Users.Select(u => u.User.Map(false)).ToList();
                    if (table.Roles?.Count > 0)
                        result.Roles = table.Roles?.Select(r => r.Map(false)).ToList();
                }

            }

            return result;

        }

        /// <summary>
        /// Maps entity to table
        /// </summary>
        /// <param name="entity">Domain entity to map</param>
        public static Scope Map(this ScopeDomain entity)
        {

            Scope result = null;

            if (entity != null)
            {
                result = new Scope(entity.Id)
                {
                    Name = entity.Name,
                    AccessKey = entity.AccessKey,
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
        public static Scope Map(this ScopeDomain entity, Scope table)
        {

            if (entity != null && table != null)
            {
                table.Name = entity.Name;
                table.AccessKey = entity.AccessKey;
                table.ChangedOn = entity.ChangedOn;
                table.ChangedBy = entity.ChangedAuthor.Id;
                table.IsActive = entity.IsActive;
            }

            return table;

        }

    }

}

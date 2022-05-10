using System.Linq;
using UserDomain = RSoft.Auth.Domain.Entities.User;
using RSoft.Auth.Infra.Data.Entities;
using RSoft.Lib.Common.ValueObjects;

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
        public static UserDomain Map(this User table)
            => Map(table, true);

        /// <summary>
        /// Maps table to entity
        /// </summary>
        /// <param name="table">Table entity to map</param>
        /// <param name="useLazy">Load related data</param>
        public static UserDomain Map(this User table, bool useLazy)
        {

            UserDomain result = null;
            if (table != null)
            {

                result = new UserDomain(table.Id)
                {
                    Document = table.Document,
                    Name = new Name(table.FirstName, table.LastName),
                    BornDate = table.BornDate,
                    Email = new Email(table.Email),
                    Type = table.Type,
                    IsActive = table.IsActive,
                    CreatedOn = table.CreatedOn,
                    ChangedOn = table.ChangedOn
                };

                if (useLazy)
                {
                    result.MapAuthor(table);
                    if (table.Credential != null)
                    {
                        result.Credential = table.Credential.Map();
                    }
                    result.ApplicationClients = table.ApplicationClients.Select(s => s.ApplicationClient.Map(false)).ToList();
                    result.Roles = table.Roles?.Select(r => r.Role.Map(false)).ToList();
                }

                //result.Validate();

            }

            return result;

        }

        /// <summary>
        /// Maps entity to table
        /// </summary>
        /// <param name="entity">Domain entity to map</param>
        public static User Map(this UserDomain entity)
            => Map(entity, true);

        /// <summary>
        /// Maps entity to table
        /// </summary>
        /// <param name="entity">Domain entity to map</param>
        /// <param name="useLazy">Load related data</param>
        public static User Map(this UserDomain entity, bool useLazy)
        {

            User result = null;

            if (entity != null)
            {

                result = new User(entity.Id)
                {
                    Document = entity.Document,
                    FirstName = entity.Name.FirstName,
                    LastName = entity.Name.LastName,
                    BornDate = entity.BornDate,
                    Email = entity.Email?.Address,
                    Type = entity.Type.Value,
                    CreatedOn = entity.CreatedOn,
                    CreatedBy = entity.CreatedAuthor.Id
                };

                if (useLazy)
                {
                    result.ApplicationClients = entity.ApplicationClients.Select(s => new UserAppClient() { AppClientId = s.Id, UserId = entity.Id }).ToList();
                    result.Roles = entity.Roles?.Select(r => new UserRole() { RoleId = r.Id, UserId = entity.Id }).ToList();
                }


            }

            return result;

        }

        /// <summary>
        /// Maps entity to an existing table
        /// </summary>
        /// <param name="entity">Domain entity to map</param>
        /// <param name="table">Instance of existing table entity</param>
        public static User Map(this UserDomain entity, User table)
        {

            if (entity != null && table != null)
            {

                table.Document = entity.Document;
                table.FirstName = entity.Name.FirstName;
                table.LastName = entity.Name.LastName;
                table.BornDate = entity.BornDate;
                table.Email = entity.Email.Address;
                table.Type = entity.Type.Value;
                table.IsActive = entity.IsActive;
                table.ChangedOn = entity.ChangedOn;
                table.ChangedBy = entity.ChangedAuthor.Id;

            }

            return table;

        }

    }

}

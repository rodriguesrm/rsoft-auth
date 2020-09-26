﻿using dmn = RSoft.Auth.Domain.Entities;
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
        {
            dmn.UserCredential result = null;

            if (table != null)
            {

                result = new dmn.UserCredential()
                {
                    UserId = table.UserId,
                    Username = table.Username,
                    UserKey = table.UserKey,
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
                    Username = entity.Username,
                    UserKey = entity.UserKey,
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
                table.Username = entity.Username;
                table.UserKey = entity.UserKey;
                table.Password = entity.Password;
                table.ChangeCredentials = entity.ChangeCredentials;
            }

            return table;

        }

    }

}

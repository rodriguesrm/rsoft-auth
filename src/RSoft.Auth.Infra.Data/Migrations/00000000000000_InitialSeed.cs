using System;
using Microsoft.EntityFrameworkCore.Migrations;
using RSoft.Auth.Infra.Data.Entities;
using RSoft.Helpers.Security;

namespace RSoft.Auth.Infra.Data.Migrations
{

    public abstract class InitialSeed : Migration
    {

        #region Local objects/variables

        private const string PWD_SUFFIX = "RSft-RL-ATPD";

        #endregion

        #region Insert/Update data

        /// <summary>
        /// Seed initial data
        /// </summary>
        /// <param name="migrationBuilder">A MicrationBuilder object instance</param>
        protected void Seed(MigrationBuilder migrationBuilder)
        {

            // Objects/Variables
            // -------------------------------------------------------

            DateTime now = DateTime.UtcNow;

            Guid scopeId = new Guid("92a4ce2a-26ed-4ae2-9813-b7e5e6a8678d");
            Guid roleId = new Guid("6e60ea33-244c-452a-ba49-d745729f8aa4");
            Guid roleServiceId = new Guid("5d41c69f-276a-4b27-ab88-ebade519504d");

            Guid userId = new Guid("745991cc-c21f-4512-ba8f-9533435b64ab");
            byte[] pwdBuffer = MD5.HashMD5($"master@soft|{PWD_SUFFIX}");
            string password = MD5.ByteArrayToString(pwdBuffer);

            Guid serviceUserId = new Guid("03f66c4a-9f5a-45c3-afa3-de6801f5592e");
            string userKey = "nP8s6QyEhqfSuGsTRqbg8MpNTFxb3SQn";

            migrationBuilder.Sql("set foreign_key_checks=0");

            // Scopes
            // -------------------------------------------------------
            migrationBuilder.InsertData
            (
                nameof(Scope),
                new string[]
                {
                    nameof(Scope.Id),
                    nameof(Scope.CreatedOn),
                    nameof(Scope.CreatedBy),
                    nameof(Scope.ChangedOn),
                    nameof(Scope.ChangedBy),
                    nameof(Scope.Name),
                    nameof(Scope.IsActive),
                    nameof(Scope.IsDeleted)
                },
                new object[] { scopeId, now, userId, null, null, "Authentication", 1, 0 }
            );

            // Roles
            // -------------------------------------------------------
            migrationBuilder.InsertData
            (
                nameof(Role),
                new string[]
                {
                    nameof(Role.Id),
                    nameof(Role.CreatedOn),
                    nameof(Role.CreatedBy),
                    nameof(Role.ChangedOn),
                    nameof(Role.ChangedBy),
                    nameof(Role.Name),
                    nameof(Role.IsActive),
                    nameof(Role.IsDeleted),
                    nameof(Role.Description),
                    nameof(Role.ScopeId)
                },
                new object[] { roleId, now, userId, null, null, "master", 1, 0, "Master privileges, has access granted to all resources", scopeId }
            );

            migrationBuilder.InsertData
            (
                nameof(Role),
                new string[]
                {
                    nameof(Role.Id),
                    nameof(Role.CreatedOn),
                    nameof(Role.CreatedBy),
                    nameof(Role.ChangedOn),
                    nameof(Role.ChangedBy),
                    nameof(Role.Name),
                    nameof(Role.IsActive),
                    nameof(Role.IsDeleted),
                    nameof(Role.Description),
                    nameof(Role.ScopeId)
                },
                new object[] { roleServiceId, now, userId, null, null, "service", 1, 0, "Service privileges, has access granted to all operations to be performed in the background", scopeId }
            );

            // Users / Credentials
            // -------------------------------------------------------
            migrationBuilder.InsertData
            (
                nameof(User),
                new string[] 
                { 
                    nameof(User.Id), 
                    nameof(User.CreatedOn), 
                    nameof(User.CreatedBy), 
                    nameof(User.ChangedOn), 
                    nameof(User.ChangedBy), 
                    nameof(User.IsActive), 
                    nameof(User.IsDeleted), 
                    nameof(User.FirstName), 
                    nameof(User.LastName), 
                    nameof(User.BornDate), 
                    nameof(User.Email) 
                },
                new object[] { userId, now, userId, null, null, 1, 0, "MASTER", "RSOFT", new DateTime(1976, 11, 13, 0, 0, 0, DateTimeKind.Utc), "master@server.com" }
            );

            migrationBuilder.InsertData
            (
                nameof(User),
                new string[]
                {
                    nameof(User.Id),
                    nameof(User.CreatedOn),
                    nameof(User.CreatedBy),
                    nameof(User.ChangedOn),
                    nameof(User.ChangedBy),
                    nameof(User.IsActive),
                    nameof(User.IsDeleted),
                    nameof(User.FirstName),
                    nameof(User.LastName),
                    nameof(User.BornDate),
                    nameof(User.Email)
                },
                new object[] { serviceUserId, now, userId, null, null, 1, 0, "SERVICES", "RSOFT", new DateTime(1976, 11, 13, 0, 0, 0, DateTimeKind.Utc), "no-reply@server.com" }
            );

            migrationBuilder.InsertData
            (
                nameof(UserCredential),
                new string[]
                {
                    nameof(UserCredential.UserId),
                    nameof(UserCredential.Username),
                    nameof(UserCredential.UserKey),
                    nameof(UserCredential.Password)
                },
                new object[] { userId, "master", null, password }
            );

            migrationBuilder.InsertData
            (
                nameof(UserCredential),
                new string[]
                {
                    nameof(UserCredential.UserId),
                    nameof(UserCredential.Username),
                    nameof(UserCredential.UserKey),
                    nameof(UserCredential.Password)
                },
                new object[] { serviceUserId, "services", userKey, null }
            );

            // User-Scopes
            // -------------------------------------------------------
            migrationBuilder.InsertData
            (
                nameof(UserScope),
                new string[] { nameof(UserScope.UserId), nameof(UserScope.ScopeId) },
                new object[] { userId, scopeId }
            );

            migrationBuilder.InsertData
            (
                nameof(UserScope),
                new string[] { nameof(UserScope.UserId), nameof(UserScope.ScopeId) },
                new object[] { serviceUserId, scopeId }
            );

            // User-Roles
            // -------------------------------------------------------
            migrationBuilder.InsertData
            (
                nameof(UserRole),
                new string[] { nameof(UserRole.UserId), nameof(UserRole.RoleId) },
                new object[] { userId, roleId }
            );

            migrationBuilder.InsertData
            (
                nameof(UserRole),
                new string[] { nameof(UserRole.UserId), nameof(UserRole.RoleId) },
                new object[] { serviceUserId, roleServiceId }
            );

            migrationBuilder.Sql("set foreign_key_checks=1");

        }

        #endregion

    }

}

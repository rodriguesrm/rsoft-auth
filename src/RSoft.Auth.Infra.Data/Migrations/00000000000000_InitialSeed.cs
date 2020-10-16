using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using RSoft.Auth.Infra.Data.Entities;
using RSoft.Framework.Cross.Enums;
using RSoft.Helpers.Security;

namespace RSoft.Auth.Infra.Data.Migrations
{

    public abstract class InitialSeed : Migration
    {

        #region Local objects/variables

        private readonly string _passwordSufix;
        private readonly bool _isProd;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new InitialSeed instance
        /// </summary>
        public InitialSeed() : base()
        {

            string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            _isProd = env.ToLower() == "production";

            IConfigurationBuilder builder =
                new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();

            IConfiguration _configuration = builder.Build();
            _passwordSufix = _configuration["Security:Secret"];
        }

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

            Guid scopeAuthId = new Guid("92a4ce2a-26ed-4ae2-9813-b7e5e6a8678d");
            Guid scopeAuthKey = new Guid("8f7318ee-4027-4cde-a6d3-529e6382f532");

            Guid scopeMailId = new Guid("1f0f52a8-aab5-4ebd-af44-15c4cead48b7");
            Guid scopeMailKey = new Guid("122b5aa2-0e8a-446f-8f05-a41236dac0e1");

            Guid scopeEvaluationId = new Guid("d2401226-754a-4535-85bc-6a3e559da66d");
            Guid scopeEvaluationKey = new Guid("f686e46b-2b6f-4568-b5e4-fec9be48dcdd");

            Guid roleAdminId = new Guid("6e60ea33-244c-452a-ba49-d745729f8aa4");
            Guid roleServiceId = new Guid("5d41c69f-276a-4b27-ab88-ebade519504d");

            Guid userId = new Guid("745991cc-c21f-4512-ba8f-9533435b64ab");
            byte[] pwdBuffer = MD5.HashMD5($"master@soft|{_passwordSufix}");
            string password = MD5.ByteArrayToString(pwdBuffer);

            migrationBuilder.Sql("set foreign_key_checks=0");

            // Scopes
            // -------------------------------------------------------
            migrationBuilder.InsertData
            (
                nameof(Scope),
                new string[]
                {
                    nameof(Scope.Id),
                    nameof(Scope.AccessKey),
                    nameof(Scope.AllowLogin),
                    nameof(Scope.CreatedOn),
                    nameof(Scope.CreatedBy),
                    nameof(Scope.ChangedOn),
                    nameof(Scope.ChangedBy),
                    nameof(Scope.Name),
                    nameof(Scope.IsActive),
                    nameof(Scope.IsDeleted)
                },
                new object[] { scopeAuthId, scopeAuthKey, true, now, userId, null, null, "Authentication", 1, 0 }
            );
            migrationBuilder.InsertData
            (
                nameof(Scope),
                new string[]
                {
                    nameof(Scope.Id),
                    nameof(Scope.AccessKey),
                    nameof(Scope.AllowLogin),
                    nameof(Scope.CreatedOn),
                    nameof(Scope.CreatedBy),
                    nameof(Scope.ChangedOn),
                    nameof(Scope.ChangedBy),
                    nameof(Scope.Name),
                    nameof(Scope.IsActive),
                    nameof(Scope.IsDeleted)
                },
                new object[] { scopeMailId, scopeMailKey, true, now, userId, null, null, "Mail Service", 1, 0 }
            );
            migrationBuilder.InsertData
            (
                nameof(Scope),
                new string[]
                {
                    nameof(Scope.Id),
                    nameof(Scope.AccessKey),
                    nameof(Scope.AllowLogin),
                    nameof(Scope.CreatedOn),
                    nameof(Scope.CreatedBy),
                    nameof(Scope.ChangedOn),
                    nameof(Scope.ChangedBy),
                    nameof(Scope.Name),
                    nameof(Scope.IsActive),
                    nameof(Scope.IsDeleted)
                },
                new object[] { scopeEvaluationId, scopeEvaluationKey, true, now, userId, null, null, "Evaluation Service", 1, 0 }
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
                new object[] { roleAdminId, now, userId, null, null, "admin", 1, 0, "Administrator privileges, has access granted to all resources", scopeAuthId }
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
                new object[] { roleServiceId, now, userId, null, null, "service", 1, 0, "Service privileges, has access granted to all operations to be performed in the background", scopeAuthId }
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
                    nameof(User.Document),
                    nameof(User.FirstName), 
                    nameof(User.LastName), 
                    nameof(User.BornDate), 
                    nameof(User.Email),
                    nameof(User.Type)
                },
                new object[] { userId, now, userId, null, null, 1, 0, "11111111111", "Admin", "RSoft", new DateTime(1976, 11, 13, 0, 0, 0, DateTimeKind.Utc), "master@server.com", (int)UserType.User }
            );

            migrationBuilder.InsertData
            (
                nameof(UserCredential),
                new string[]
                {
                    nameof(UserCredential.UserId),
                    nameof(UserCredential.Login),
                    nameof(UserCredential.Password),
                    nameof(UserCredential.ChangeCredentials),
                    nameof(UserCredential.AuthFailsQty),
                    nameof(UserCredential.LockoutUntil)
                },
                new object[] { userId, "admin", password, _isProd, 0, null }
            );

            // User-Scopes
            // -------------------------------------------------------
            migrationBuilder.InsertData
            (
                nameof(UserScope),
                new string[] { nameof(UserScope.UserId), nameof(UserScope.ScopeId) },
                new object[] { userId, scopeAuthId }
            );

            // User-Roles
            // -------------------------------------------------------
            migrationBuilder.InsertData
            (
                nameof(UserRole),
                new string[] { nameof(UserRole.UserId), nameof(UserRole.RoleId) },
                new object[] { userId, roleAdminId }
            );

            migrationBuilder.Sql("set foreign_key_checks=1");

        }

        #endregion

    }

}

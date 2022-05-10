using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using RSoft.Auth.Cross.Common.Options;
using RSoft.Auth.Infra.Data.Entities;
using RSoft.Helpers.Security;
using RSoft.Lib.Common.Enums;

namespace RSoft.Auth.Infra.Data.Migrations
{

    public abstract class InitialSeed : Migration
    {

        #region Local objects/variables

        private readonly bool _isProd;
        private readonly SecurityOptions _securityOptions;

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

            IConfiguration configuration = builder.Build();
            _securityOptions = new SecurityOptions();
            configuration.GetSection("Application:Security").Bind(_securityOptions);
        }

        #endregion

        #region Insert/Update data

        private string GenerateUserDocument()
        {
            System.Threading.Thread.Sleep(150);
            return DateTime.UtcNow.ToString("yy MM dd HH mm ss ff").Replace(" ", string.Empty);
        }

        /// <summary>
        /// Seed initial data
        /// </summary>
        /// <param name="migrationBuilder">A MicrationBuilder object instance</param>
        protected void Seed(MigrationBuilder migrationBuilder)
        {

            // Objects/Variables
            // -------------------------------------------------------

            DateTime now = DateTime.UtcNow;

            Guid authClientId = new("92a4ce2a-26ed-4ae2-9813-b7e5e6a8678d");
            Guid authClientSecret = new("8f7318ee-4027-4cde-a6d3-529e6382f532");

            Guid mailClientId = new("1f0f52a8-aab5-4ebd-af44-15c4cead48b7");
            Guid mailClientSecret = new("122b5aa2-0e8a-446f-8f05-a41236dac0e1");

            Guid personClientId = new("d2401226-754a-4535-85bc-6a3e559da66d");
            Guid personClientSecret = new("f686e46b-2b6f-4568-b5e4-fec9be48dcdd");

            Guid entryClientId = new("3f3b94db-d868-4cb3-8098-214a53eccc35");
            Guid entryClientSecret = new("cda09ab8-2b05-49e8-8eec-60ad6cfea2e5");

            Guid allocateClientId = new("c57c65ae-379f-4790-9eff-ef514cab9da1");
            Guid allocateClientSecret = new("db70b5a7-a67c-4a12-a176-7a3362e2a71e");

            Guid userAdminRoleId = new("6e60ea33-244c-452a-ba49-d745729f8aa4");
            Guid serviceRoleId = new("5d41c69f-276a-4b27-ab88-ebade519504d");

            Guid userId = new("745991cc-c21f-4512-ba8f-9533435b64ab");
            byte[] pwdBuffer = MD5.HashMD5($"master@soft|{_securityOptions.Secret}");
            string password = MD5.ByteArrayToString(pwdBuffer);

            migrationBuilder.Sql("set foreign_key_checks=0");

            // Application-Clients
            // -------------------------------------------------------
            migrationBuilder.InsertData
            (
                nameof(AppClient),
                new string[]
                {
                    nameof(AppClient.Id),
                    nameof(AppClient.AccessKey),
                    nameof(AppClient.AllowLogin),
                    nameof(AppClient.CreatedOn),
                    nameof(AppClient.CreatedBy),
                    nameof(AppClient.ChangedOn),
                    nameof(AppClient.ChangedBy),
                    nameof(AppClient.Name),
                    nameof(AppClient.IsActive),
                    nameof(AppClient.IsDeleted)
                },
                new object[] { authClientId, authClientSecret, true, now, userId, null, null, "Authentication Service", 1, 0 }
            );
            migrationBuilder.InsertData
            (
                nameof(AppClient),
                new string[]
                {
                    nameof(AppClient.Id),
                    nameof(AppClient.AccessKey),
                    nameof(AppClient.AllowLogin),
                    nameof(AppClient.CreatedOn),
                    nameof(AppClient.CreatedBy),
                    nameof(AppClient.ChangedOn),
                    nameof(AppClient.ChangedBy),
                    nameof(AppClient.Name),
                    nameof(AppClient.IsActive),
                    nameof(AppClient.IsDeleted)
                },
                new object[] { mailClientId, mailClientSecret, true, now, userId, null, null, "Mail Service", 1, 0 }
            );
            migrationBuilder.InsertData
            (
                nameof(AppClient),
                new string[]
                {
                    nameof(AppClient.Id),
                    nameof(AppClient.AccessKey),
                    nameof(AppClient.AllowLogin),
                    nameof(AppClient.CreatedOn),
                    nameof(AppClient.CreatedBy),
                    nameof(AppClient.ChangedOn),
                    nameof(AppClient.ChangedBy),
                    nameof(AppClient.Name),
                    nameof(AppClient.IsActive),
                    nameof(AppClient.IsDeleted)
                },
                new object[] { personClientId, personClientSecret, true, now, userId, null, null, "Person Service", 1, 0 }
            );
            migrationBuilder.InsertData
            (
                nameof(AppClient),
                new string[]
                {
                    nameof(AppClient.Id),
                    nameof(AppClient.AccessKey),
                    nameof(AppClient.AllowLogin),
                    nameof(AppClient.CreatedOn),
                    nameof(AppClient.CreatedBy),
                    nameof(AppClient.ChangedOn),
                    nameof(AppClient.ChangedBy),
                    nameof(AppClient.Name),
                    nameof(AppClient.IsActive),
                    nameof(AppClient.IsDeleted)
                },
                new object[] { entryClientId, entryClientSecret, true, now, userId, null, null, "Entry Service", 1, 0 }
            );
            migrationBuilder.InsertData
            (
                nameof(AppClient),
                new string[]
                {
                    nameof(AppClient.Id),
                    nameof(AppClient.AccessKey),
                    nameof(AppClient.AllowLogin),
                    nameof(AppClient.CreatedOn),
                    nameof(AppClient.CreatedBy),
                    nameof(AppClient.ChangedOn),
                    nameof(AppClient.ChangedBy),
                    nameof(AppClient.Name),
                    nameof(AppClient.IsActive),
                    nameof(AppClient.IsDeleted)
                },
                new object[] { allocateClientId, allocateClientSecret, true, now, userId, null, null, "Allocate Service", 1, 0 }
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
                    nameof(Role.ApplicationClientId)
                },
                new object[] { userAdminRoleId, now, userId, null, null, "admin", 1, 0, "Administrator privileges, has access granted to all resources", authClientId }
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
                    nameof(Role.ApplicationClientId)
                },
                new object[] { serviceRoleId, now, userId, null, null, "service", 1, 0, "Service privileges, has access granted to all operations to be performed in the background", authClientId }
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
                new object[] { authClientId, now, userId, null, null, 0, 0, GenerateUserDocument(), "Authentication", "Service", DateTime.UtcNow, "auth@service.na", (int)UserType.Service }
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
                    nameof(User.Document),
                    nameof(User.FirstName),
                    nameof(User.LastName),
                    nameof(User.BornDate),
                    nameof(User.Email),
                    nameof(User.Type)
                },
                new object[] { mailClientId, now, userId, null, null, 0, 0, GenerateUserDocument(), "Mail", "Service", DateTime.UtcNow, "mail@service.na", (int)UserType.Service }
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
                    nameof(User.Document),
                    nameof(User.FirstName),
                    nameof(User.LastName),
                    nameof(User.BornDate),
                    nameof(User.Email),
                    nameof(User.Type)
                },
                new object[] { personClientId, now, userId, null, null, 0, 0, GenerateUserDocument(), "Person", "Service", DateTime.UtcNow, "person@service.na", (int)UserType.Service }
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
                    nameof(User.Document),
                    nameof(User.FirstName),
                    nameof(User.LastName),
                    nameof(User.BornDate),
                    nameof(User.Email),
                    nameof(User.Type)
                },
                new object[] { entryClientId, now, userId, null, null, 0, 0, GenerateUserDocument(), "Entry", "Service", DateTime.UtcNow, "entry@service.na", (int)UserType.Service }
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
                    nameof(User.Document),
                    nameof(User.FirstName),
                    nameof(User.LastName),
                    nameof(User.BornDate),
                    nameof(User.Email),
                    nameof(User.Type)
                },
                new object[] { allocateClientId, now, userId, null, null, 0, 0, GenerateUserDocument(), "Allocate", "Service", DateTime.UtcNow, "allocate@service.na", (int)UserType.Service }
            );

            // User-Application-Client
            // -------------------------------------------------------
            migrationBuilder.InsertData
            (
                nameof(UserAppClient),
                new string[] { nameof(UserAppClient.UserId), nameof(UserAppClient.AppClientId) },
                new object[] { userId, authClientId }
            );

            // User-Roles
            // -------------------------------------------------------
            migrationBuilder.InsertData
            (
                nameof(UserRole),
                new string[] { nameof(UserRole.UserId), nameof(UserRole.RoleId) },
                new object[] { userId, userAdminRoleId }
            );

            migrationBuilder.Sql("set foreign_key_checks=1");

        }

        #endregion

    }

}

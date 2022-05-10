using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RSoft.Auth.Infra.Data.Migrations
{
    public partial class InitialCreateDb : InitialSeed
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IsActive = table.Column<ulong>(type: "bit", nullable: false),
                    IsDeleted = table.Column<ulong>(type: "bit", nullable: false, defaultValue: 0ul),
                    Document = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FirstName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BornDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Email = table.Column<string>(type: "varchar(254)", unicode: false, maxLength: 254, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ChangedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ChangedBy = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_ChangedAuthor",
                        column: x => x.ChangedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_CreatedAuthor",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AppClient",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AccessKey = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AllowLogin = table.Column<ulong>(type: "bit", nullable: false),
                    IsActive = table.Column<ulong>(type: "bit", nullable: false),
                    IsDeleted = table.Column<ulong>(type: "bit", nullable: false, defaultValue: 0ul),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ChangedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ChangedBy = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppClient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppClient_ChangedAuthor",
                        column: x => x.ChangedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppClient_CreatedAuthor",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserCredential",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Login = table.Column<string>(type: "varchar(254)", unicode: false, maxLength: 254, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "varchar(32)", unicode: false, maxLength: 32, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ChangeCredentials = table.Column<ulong>(type: "bit", nullable: false),
                    AuthFailsQty = table.Column<int>(type: "int", nullable: false),
                    LockoutUntil = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCredential", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserCredential_User",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserCredentialToken",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ExpiresOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FirstAccess = table.Column<ulong>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCredentialToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCredentialToken_User",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IsActive = table.Column<ulong>(type: "bit", nullable: false),
                    IsDeleted = table.Column<ulong>(type: "bit", nullable: false, defaultValue: 0ul),
                    Description = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ApplicationClientId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ChangedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ChangedBy = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Role_AppClient",
                        column: x => x.ApplicationClientId,
                        principalTable: "AppClient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Role_ChangedAuthor",
                        column: x => x.ChangedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Role_CreatedAuthor",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserAppClient",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AppClientId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAppClient", x => new { x.UserId, x.AppClientId });
                    table.ForeignKey(
                        name: "FK_UserAppClient_AppClient",
                        column: x => x.AppClientId,
                        principalTable: "AppClient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAppClient_User",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRole_Role",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_User",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "AK_AppClient_AccessKey",
                table: "AppClient",
                column: "AccessKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "AK_AppClient_Name",
                table: "AppClient",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppClient_ChangedBy",
                table: "AppClient",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AppClient_ChangedOn",
                table: "AppClient",
                column: "ChangedOn");

            migrationBuilder.CreateIndex(
                name: "IX_AppClient_CreatedBy",
                table: "AppClient",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AppClient_CreatedOn",
                table: "AppClient",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "AK_Role_Name",
                table: "Role",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Role_ApplicationClientId",
                table: "Role",
                column: "ApplicationClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_ChangedBy",
                table: "Role",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Role_ChangedOn",
                table: "Role",
                column: "ChangedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Role_CreatedBy",
                table: "Role",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Role_CreatedOn",
                table: "Role",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "AK_User_Document",
                table: "User",
                column: "Document",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "AK_User_Email",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_ChangedBy",
                table: "User",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_User_ChangedOn",
                table: "User",
                column: "ChangedOn");

            migrationBuilder.CreateIndex(
                name: "IX_User_CreatedBy",
                table: "User",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_User_CreatedOn",
                table: "User",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_User_FullName",
                table: "User",
                columns: new[] { "FirstName", "LastName" });

            migrationBuilder.CreateIndex(
                name: "IX_UserAppClient_AppClientId",
                table: "UserAppClient",
                column: "AppClientId");

            migrationBuilder.CreateIndex(
                name: "AK_UserCredential_Login",
                table: "UserCredential",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserCredentialToken_UserId",
                table: "UserCredentialToken",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");

            Seed(migrationBuilder);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAppClient");

            migrationBuilder.DropTable(
                name: "UserCredential");

            migrationBuilder.DropTable(
                name: "UserCredentialToken");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "AppClient");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}

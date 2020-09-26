using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RSoft.Auth.Infra.Data.Migrations
{
    public partial class InitialCreate : InitialSeed
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    ChangedOn = table.Column<DateTime>(nullable: true),
                    ChangedBy = table.Column<Guid>(nullable: true),
                    IsActive = table.Column<ulong>(type: "bit", nullable: false),
                    IsDeleted = table.Column<ulong>(type: "bit", nullable: false, defaultValue: 0ul),
                    FirstName = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    LastName = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    BornDate = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(unicode: false, maxLength: 254, nullable: false),
                    Type = table.Column<int>(nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "Scope",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    ChangedOn = table.Column<DateTime>(nullable: true),
                    ChangedBy = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Key = table.Column<Guid>(nullable: false),
                    IsActive = table.Column<ulong>(type: "bit", nullable: false),
                    IsDeleted = table.Column<ulong>(type: "bit", nullable: false, defaultValue: 0ul)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scope", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Scope_ChangedAuthor",
                        column: x => x.ChangedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Scope_CreatedAuthor",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserCredential",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    Username = table.Column<string>(unicode: false, maxLength: 254, nullable: false),
                    UserKey = table.Column<Guid>(nullable: true),
                    Password = table.Column<string>(unicode: false, maxLength: 32, nullable: true),
                    ChangeCredentials = table.Column<ulong>(type: "bit", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "UserCredentialToken",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ExpiresOn = table.Column<DateTime>(nullable: false),
                    FirstAccess = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCredentialToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCredentialToken_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    ChangedOn = table.Column<DateTime>(nullable: true),
                    ChangedBy = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    IsActive = table.Column<ulong>(type: "bit", nullable: false),
                    IsDeleted = table.Column<ulong>(type: "bit", nullable: false, defaultValue: 0ul),
                    Description = table.Column<string>(unicode: false, maxLength: 150, nullable: false),
                    ScopeId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
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
                    table.ForeignKey(
                        name: "FK_Role_Scope",
                        column: x => x.ScopeId,
                        principalTable: "Scope",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserScope",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    ScopeId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserScope", x => new { x.UserId, x.ScopeId });
                    table.ForeignKey(
                        name: "FK_UserScope_Scope",
                        column: x => x.ScopeId,
                        principalTable: "Scope",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserScope_User",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
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
                });

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
                name: "AK_Role_Name",
                table: "Role",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Role_ScopeId",
                table: "Role",
                column: "ScopeId");

            migrationBuilder.CreateIndex(
                name: "IX_Scope_ChangedBy",
                table: "Scope",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Scope_ChangedOn",
                table: "Scope",
                column: "ChangedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Scope_CreatedBy",
                table: "Scope",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Scope_CreatedOn",
                table: "Scope",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "AK_Scope_Key",
                table: "Scope",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "AK_Scope_Name",
                table: "Scope",
                column: "Name",
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
                name: "AK_User_Email",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_FullName",
                table: "User",
                columns: new[] { "FirstName", "LastName" });

            migrationBuilder.CreateIndex(
                name: "AK_UserCredential_UserKey",
                table: "UserCredential",
                column: "UserKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "AK_UserCredential_Username",
                table: "UserCredential",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserCredentialToken_UserId",
                table: "UserCredentialToken",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserScope_ScopeId",
                table: "UserScope",
                column: "ScopeId");

            Seed(migrationBuilder);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserCredential");

            migrationBuilder.DropTable(
                name: "UserCredentialToken");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "UserScope");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Scope");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}

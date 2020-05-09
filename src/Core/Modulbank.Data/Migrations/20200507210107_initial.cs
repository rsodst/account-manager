using System.Data;
using FluentMigrator;

namespace Modulbank.Data.Migrations
{
    [Migration(20200507210107)]
    public class InitialMigration : Migration
    {
        public override void Up()
        {
            Create.Table("Roles")
                .WithColumn("Id").AsGuid().PrimaryKey("PK_Roles").NotNullable()
                .WithColumn("Name").AsString(256).Nullable()
                .WithColumn("NormalizedName").AsString(256).Nullable().Indexed("RoleNameIndex").Unique()
                .WithColumn("ConcurrencyStamp").AsString(256).Nullable();

            Create.Table("Users")
                .WithColumn("Id").AsGuid().PrimaryKey("PK_Users").NotNullable()
                .WithColumn("UserName").AsString(256).Nullable()
                .WithColumn("NormalizedUserName").AsString(256).NotNullable().Indexed("UserNameIndex").Unique()
                .WithColumn("Email").AsString(256).Nullable()
                .WithColumn("NormalizedEmail").AsString(256).Nullable().Indexed("EmailIndex")
                .WithColumn("EmailConfirmed").AsBoolean().NotNullable()
                .WithColumn("PasswordHash").AsString(256).Nullable()
                .WithColumn("SecurityStamp").AsString(256).Nullable()
                .WithColumn("ConcurrencyStamp").AsString(256).Nullable()
                .WithColumn("PhoneNumber").AsString(256).Nullable()
                .WithColumn("PhoneNumberConfirmed").AsBoolean().NotNullable()
                .WithColumn("TwoFactorEnabled").AsBoolean().NotNullable()
                .WithColumn("LockoutEnd").AsDateTimeOffset().Nullable()
                .WithColumn("LockoutEnabled").AsBoolean().NotNullable()
                .WithColumn("AccessFailedCount").AsInt32().NotNullable();

            Create.Table("RoleClaims")
                .WithColumn("Id").AsGuid().NotNullable()
                .PrimaryKey("PK_AspNetRoleClaims").Identity()
                .WithColumn("RoleId").AsGuid().NotNullable().Indexed("IX_RoleClaims_RoleId")
                .ForeignKey("FK_RoleClaims_Roles_RoleId", "Roles", "Id")
                .OnDelete(Rule.Cascade)
                .WithColumn("ClaimType").AsString().Nullable()
                .WithColumn("ClaimValue").AsString().Nullable();

            Create.Table("UserClaims")
                .WithColumn("Id").AsGuid().NotNullable()
                .PrimaryKey("PK_UserClaims").Identity()
                .WithColumn("UserId").AsGuid().NotNullable().Indexed("IX_UserClaims_UserId")
                .ForeignKey("FK_UserClaims_Users_UserId", "Users", "Id")
                .OnDelete(Rule.Cascade)
                .WithColumn("ClaimType").AsString().Nullable()
                .WithColumn("ClaimValue").AsString().Nullable();

            Create.Table("UserLogins")
                .WithColumn("LoginProvider").AsString().NotNullable()
                .PrimaryKey("PK_UserLogins")
                .WithColumn("ProviderKey").AsString().NotNullable()
                .PrimaryKey("PK_UserLogins")
                .WithColumn("ProviderDisplayName").AsString().Nullable()
                .WithColumn("UserId").AsGuid().NotNullable()
                .Indexed("IX_UserLogins_UserId")
                .ForeignKey("FK_UserLogins_Users_UserId", "Users", "Id")
                .OnDelete(Rule.Cascade);

            Create.Table("UserRoles")
                .WithColumn("UserId").AsGuid().NotNullable()
                .Indexed("IX_UserRoles_UserId")
                .PrimaryKey("PK_UserRoles")
                .ForeignKey("FK_UserRoles_Users_UserId", "Users", "Id")
                .OnDelete(Rule.Cascade)
                .WithColumn("RoleId").AsGuid().NotNullable()
                .Indexed("IX_UserRoles_RoleId")
                .PrimaryKey("PK_UserRoles")
                .ForeignKey("FK_UserRoles_Roles_RoleId", "Roles", "Id")
                .OnDelete(Rule.Cascade);

            Create.Table("UserTokens")
                .WithColumn("UserId").AsGuid().NotNullable()
                .PrimaryKey("PK_UserTokens")
                .ForeignKey("FK_UserTokens_Users_UserId", "Users", "Id")
                .OnDelete(Rule.Cascade)
                .WithColumn("LoginProvider ").AsString().NotNullable()
                .PrimaryKey("PK_UserTokens")
                .WithColumn("Name").AsString().NotNullable()
                .PrimaryKey("PK_UserTokens")
                .WithColumn("Value").AsString().Nullable();
        }

        public override void Down()
        {
            Delete.Table("RoleClaims");

            Delete.Table("UserClaims");

            Delete.Table("UserLogins");

            Delete.Table("UserRoles");

            Delete.Table("UserTokens");

            Delete.Table("Roles");

            Delete.Table("Users");

            Delete.Index("RoleNameIndex");

            Delete.Index("UserNameIndex");

            Delete.Index("IX_RoleClaims_RoleId");

            Delete.Index("IX_UserClaims_UserId");

            Delete.Index("IX_UserLogins_UserId");

            Delete.Index("IX_UserRoles_UserId");

            Delete.Index("IX_UserRoles_RoleId");
        }
    }
}
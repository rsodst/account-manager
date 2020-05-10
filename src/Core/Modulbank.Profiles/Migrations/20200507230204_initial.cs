using FluentMigrator;

namespace Modulbank.Profiles.Migrations
{
    [Migration(20200507230204)]
    public class ModulBankProfileInitial : Migration
    {
        public override void Up()
        {
            Create.Table("PersonDetails")
                .WithColumn("Id").AsGuid().PrimaryKey("PK_PersonDetails").NotNullable()
                .WithColumn("UserId").AsGuid().NotNullable().Unique()
                .WithColumn("FirstName").AsString().NotNullable()
                .WithColumn("LastName").AsString().Nullable()
                .WithColumn("MiddleName").AsString().Nullable()
                .WithColumn("CreationDate").AsDateTime2().NotNullable()
                .WithColumn("LastModified").AsDateTime2().NotNullable();

            Create.Table("PersonPhoto")
                .WithColumn("Id").AsGuid().PrimaryKey("PK_PersonPhoto").NotNullable()
                .WithColumn("UserId").AsGuid().NotNullable().Unique()
                .WithColumn("FileName").AsString().NotNullable()
                .WithColumn("CreationDate").AsDateTime2().NotNullable()
                .WithColumn("LastModified").AsDateTime2().NotNullable();

            Create.Table("ProfileConfirmation")
                .WithColumn("Id").AsGuid().PrimaryKey("PK_ProfileConfirmation").NotNullable()
                .WithColumn("UserId").AsGuid().NotNullable().Unique()
                .WithColumn("CreationDate").AsDateTime2().NotNullable()
                .WithColumn("LastModified").AsDateTime2().NotNullable()
                .WithColumn("Description").AsString().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("PersonDetails");

            Delete.Table("PersonPhoto");

            Delete.Table("ProfileConfirmation");
        }
    }
}
using FluentMigrator;

namespace Modulbank.Profiles.Migrations
{
    [Migration(20200506534313)]
    public class ModulBankAccountsInitial : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
            
            CREATE SEQUENCE seq_name START 4000000000;

            CREATE TABLE public.""Accounts""
            (
            ""Id"" uuid NOT NULL,
            ""Number"" bigint NOT NULL DEFAULT nextval('seq_name'),
            ""UserId"" uuid NOT NULL,
            ""IsDeleted"" boolean NOT NULL,
            ""Balance"" numeric(19,5) NOT NULL,
            ""CreationDate"" timestamp without time zone NOT NULL,
            ""ExpiredDate"" timestamp without time zone NOT NULL,
            ""LastModified"" timestamp without time zone NOT NULL,
                CONSTRAINT ""PK_Accounts"" PRIMARY KEY (""Id"")
                )

            TABLESPACE pg_default;

            ALTER TABLE public.""Accounts""
            OWNER to postgres;
            ");

            Create.Table("AccountActions")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Type").AsInt32().NotNullable()
                .WithColumn("CreationDate").AsDateTime2().NotNullable();

            Create.Table("Transactions")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("UserId").AsGuid().NotNullable()
                .WithColumn("WriteOffAccount").AsGuid().NotNullable()
                .WithColumn("DestinationAccount").AsGuid().NotNullable()
                .WithColumn("Type").AsInt32().NotNullable()
                .WithColumn("Status").AsInt32().NotNullable()
                .WithColumn("CreationDate").AsDateTime2().NotNullable()
                .WithColumn("ProceedDate").AsDateTime2().NotNullable();

            Create.Table("AccountDetails")
                .WithColumn("AccountId").AsGuid().NotNullable()
                .ForeignKey("Accounts", "Id")
                .WithColumn("LimitByOperation").AsDecimal().NotNullable()
                .WithColumn("Currency").AsInt32().NotNullable()
                .WithColumn("Description").AsString().Nullable()
                .WithColumn("CreationDate").AsDateTime2().NotNullable()
                .WithColumn("LastModified").AsDateTime2().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("Accounts");

            Delete.Table("AccountActions");

            Delete.Table("Transactions");
        }
    }
}
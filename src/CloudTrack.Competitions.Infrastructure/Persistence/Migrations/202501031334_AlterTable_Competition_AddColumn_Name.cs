using FluentMigrator;

namespace CloudTrack.Competitions.Infrastructure.Persistence.Migrations;

[Migration(202501031334)]
public class _202501031334_AlterTable_Competition_AddColumn_Name : Migration
{
    public override void Down()
    {
        Delete.Column("name").FromTable("competitions");
    }

    public override void Up()
    {
        Alter.Table("competitions")
            .AddColumn("name").AsAnsiString(250).Nullable();

        Execute.Sql("update competitions set name = city || ' ' || date(\"startAt\") where name is null");

        Alter.Column("name").OnTable("competitions")
            .AsAnsiString(250).NotNullable();
    }
}

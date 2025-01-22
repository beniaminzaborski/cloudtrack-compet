﻿using FluentMigrator;

namespace CloudTrack.Competitions.Infrastructure.Persistence.Migrations;

[Migration(202301301631)]
public class _202301301631_CreateTable_Competition : Migration
{
    public override void Down()
    {
        Delete.Table("competitions");
    }

    public override void Up()
    {
        Create.Table("competitions")
            .WithColumn("id").AsGuid().NotNullable()
            .WithColumn("distanceAmount").AsDecimal().NotNullable()
            .WithColumn("distanceUnit").AsByte().NotNullable()
            .WithColumn("city").AsAnsiString(100).NotNullable()
            .WithColumn("placeLongitude").AsDecimal(11, 8).NotNullable()
            .WithColumn("placeLatitude").AsDecimal(10, 8).NotNullable()
            .WithColumn("startAt").AsDateTime().NotNullable()
            .WithColumn("maxCompetitors").AsInt32().NotNullable()
            .WithColumn("status").AsByte().NotNullable();

        Create.PrimaryKey($"PK__competitions__id")
            .OnTable("competitions").Column("id");
    }
}

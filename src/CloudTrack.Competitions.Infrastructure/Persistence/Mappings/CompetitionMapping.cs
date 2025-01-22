﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using CloudTrack.Competitions.Domain.ManagingCompetition;

namespace CloudTrack.Competitions.Infrastructure.Persistence.Mappings;

internal class CompetitionMapping : IEntityTypeConfiguration<Competition>
{
    public void Configure(EntityTypeBuilder<Competition> builder)
    {
        builder.ToTable(name: "competitions");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasConversion(entityId => entityId.Value, dbId => new CompetitionId(dbId));

        builder.Property(e => e.Name)
            .HasColumnName("name");

        builder.OwnsOne(e => e.Distance,
            navigationBuilder => 
            {
                navigationBuilder
                    .Property(distance => distance.Amount)
                    .HasColumnName("distanceAmount");
                navigationBuilder
                    .Property(distance => distance.Unit)
                    .HasColumnName("distanceUnit")
                    .HasColumnType("tinyint");
            });

        builder.OwnsOne(e => e.Place,
            navigationBuilder =>
            {
                navigationBuilder
                    .Property(place => place.City)
                    .HasColumnName("city");
                navigationBuilder.OwnsOne(p => p.Localization,
                    placeNavigationBuilder =>
                    {
                        placeNavigationBuilder
                            .Property(loc => loc.Latitude)
                            .HasColumnName("placeLatitude");
                        placeNavigationBuilder
                            .Property(loc => loc.Longitude)
                            .HasColumnName("placeLongitude");
                    });
                //navigationBuilder
                //    .Property(place => place.Longitute)
                //    .HasColumnName("placeLongitute");
                //navigationBuilder
                //    .Property(place => place.Latitude)
                //    .HasColumnName("placeLatitude");
            });

        builder.Property(e => e.StartAt).HasColumnName("startAt");
        builder.Property(e => e.MaxCompetitors).HasColumnName("maxCompetitors");
        builder.Property(e => e.Status).HasColumnName("status").HasColumnType("tinyint");

        builder.HasMany(e => e.Checkpoints)
               .WithOne()
               .IsRequired()
               .HasForeignKey(c => c.CompetitionId)
               .OnDelete(DeleteBehavior.Cascade);
        builder.Metadata
               .FindNavigation("Checkpoints")
               .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}

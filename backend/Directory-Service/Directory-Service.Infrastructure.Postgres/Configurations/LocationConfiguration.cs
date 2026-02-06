using Directory_Service.Domain.Location;
using Directory_Service.Domain.Location.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Directory_Service.Infrastructure.Configurations;

public static class Index
{
    public static string NAME = "ix_location_name";
    public static string ADDRESS = "ix_locations_address";
}

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.ToTable("location");

        builder.HasKey(l => l.Id).HasName("pk_location");

        builder.Property(l => l.Id)
            .HasConversion(l => l.Value, l => new LocationId(l))
            .HasColumnName("id");

        builder.Property(l => l.Name)
            .HasConversion(l => l.Value, l => new Name(l))
            .IsRequired()
            .HasColumnName("name");

        builder.OwnsOne(l => l.Address, nb =>
        {
            nb.ToJson();
            
            nb.Property(x => x.Building).IsRequired().HasColumnName("building");
            nb.Property(x => x.City).IsRequired().HasColumnName("city");
            nb.Property(x => x.Flat).IsRequired().HasColumnName("flat");
            nb.Property(x => x.Street).IsRequired().HasColumnName("street");
        });

        builder.Property(l => l.Timezone)
            .HasConversion(l => l.Value, l => new Timezone(l))
            .IsRequired()
            .HasColumnName("timezone");

        builder.HasMany(l => l.DepartmentsLocations)
            .WithOne()
            .HasForeignKey(l => l.LocationId);

        builder.Property(l => l.IsActive).HasColumnName("is_active");

        builder.Property(l => l.CreatedAt).HasColumnName("created_at");

        builder.Property(l => l.UpdatedAt).HasColumnName("updated_at");

        builder.HasIndex(x => x.Name).IsUnique().HasDatabaseName(Index.NAME);
    }
}
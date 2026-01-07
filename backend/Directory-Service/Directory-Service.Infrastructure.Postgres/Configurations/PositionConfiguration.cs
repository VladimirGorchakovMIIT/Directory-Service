using Directory_Service.Domain.Position;
using Directory_Service.Domain.Position.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Directory_Service.Infrastructure.Configurations;

public class PositionConfiguration : IEntityTypeConfiguration<Position>
{
    public void Configure(EntityTypeBuilder<Position> builder)
    {
        builder.ToTable("position");
        
        builder.HasKey(p => p.Id).HasName("pk_position");

        builder.Property(p => p.Id)
            .HasConversion(p => p.Value, p => new PositionId(p))
            .HasColumnName("id");
        
        builder.Property(p => p.Name)
            .HasConversion(p => p.Value, p => new Name(p))
            .HasColumnName("name");
        
        builder.Property(p => p.Description)
            .HasConversion(p => p.Value, p => new Description(p))
            .HasColumnName("description");
        
        builder.Property(p => p.IsActive)
            .IsRequired()
            .HasColumnName("active");

        builder.Property(p => p.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");
        
        builder.Property(p => p.UpdatedAt)
            .IsRequired()
            .HasColumnName("updated_at");

        builder.HasMany(p => p.DepartmentPosition)
            .WithOne()
            .HasForeignKey(dp => dp.PositionId);
    }
}
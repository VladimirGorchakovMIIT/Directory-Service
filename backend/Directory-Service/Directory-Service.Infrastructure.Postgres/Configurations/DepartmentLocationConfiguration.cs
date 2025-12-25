using Directory_Service.Domain.Department;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Directory_Service.Infrastructure.Configurations;

public class DepartmentLocationConfiguration : IEntityTypeConfiguration<DepartmentLocation>
{
    public void Configure(EntityTypeBuilder<DepartmentLocation> builder)
    {
        builder.ToTable("department_location");
        
        builder.HasKey(d => d.DepartmentLocationId).HasName("pk_department_location");

        builder.Property(dl => dl.DepartmentLocationId)
            .HasConversion(dl => dl.Value, dl => new DepartmentLocationId(dl))
            .HasColumnName("department_location_id");        
        
        builder.Property(dl => dl.CreatedAt).HasColumnName("created_at");
        
        builder.HasOne(dl => dl.Department).WithMany(d => d.DepartmentLocations)
            .HasForeignKey("department_id")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(dl => dl.Location).WithMany(l => l.DepartmentsLocations)
            .HasForeignKey("location_id")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
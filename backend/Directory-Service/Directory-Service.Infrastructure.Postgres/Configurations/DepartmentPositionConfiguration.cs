using Directory_Service.Domain.Department;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Directory_Service.Infrastructure.Configurations;

public class DepartmentPositionConfiguration : IEntityTypeConfiguration<DepartmentPosition>
{
    public void Configure(EntityTypeBuilder<DepartmentPosition> builder)
    {
        builder.ToTable("department_position");
        
        builder.HasKey(x => x.Id).HasName("pk_department_position");
        
        builder.Property(dp => dp.Id)
            .HasConversion(dp => dp.Value, dp => new DepartmentPositionId(dp))
            .HasColumnName("department_position_id");
        
        builder.Property(dp => dp.CreatedAt).HasColumnName("created_at");
        
        builder.HasOne(dp => dp.Department).WithMany(d => d.DepartmentPositions)
            .HasForeignKey("department_id")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(dp => dp.Position).WithMany(dp => dp.DepartmentPosition)
            .HasForeignKey("position_id")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
    }
}
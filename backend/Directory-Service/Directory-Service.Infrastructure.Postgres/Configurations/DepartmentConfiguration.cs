using Directory_Service.Domain.Department;
using Directory_Service.Domain.Department.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Path = Directory_Service.Domain.Department.ValueObjects.Path;

namespace Directory_Service.Infrastructure.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("department");
        builder.HasKey(x => x.Id).HasName("pk_department");
        
        builder.Property(x => x.Id)
            .HasConversion(d => d.Value, d => new DepartmentId(d))
            .HasColumnName("id");
        
        builder.Property(p => p.ParentId)
            .HasConversion(p => p!.Value, p => new DepartmentId(p))
            .HasColumnName("parent_id");

        builder.Property(n => n.Name)
            .HasConversion(n => n.Value, d => new Name(d))
            .HasColumnName("name");
        
        builder.Property(d => d.Identifier)
            .HasConversion(d => d.Value, d => new Identifier(d))
            .HasColumnName("identifier");
        
        builder.Property(d => d.Path)
            .HasConversion(d => d.Value, d => new Path(d))
            .HasColumnName("path");
        
        builder.Property(d => d.Depth)
            .HasConversion(d => d.Value, d => new Depth(d))
            .HasColumnName("depth");
        
        builder.Property(d => d.IsActive).HasColumnName("is_active");
        
        builder.Property(d => d.CreatedAt).HasColumnName("created_at");
        
        builder.Property(d => d.UpdatedAt).HasColumnName("updated_at");

        builder.HasOne(d => d.Parent).WithMany(p => p.DepartmentsChild)
            .HasForeignKey(d => d.ParentId);
    }
}
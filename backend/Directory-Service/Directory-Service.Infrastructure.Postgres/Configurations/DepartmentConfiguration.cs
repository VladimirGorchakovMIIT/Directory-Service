using Directory_Service.Domain.Department;
using Directory_Service.Domain.Department.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Path = Directory_Service.Domain.Department.ValueObjects.Path;

namespace Directory_Service.Infrastructure.Configurations;

public static class DepartmentIndex
{
    public static string IDENTIFIER = "ix_department_identifier";
}

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("department");
        builder.HasKey(x => x.DepartmentId).HasName("pk_department");

        builder.Property(x => x.Depth).HasColumnName("depth"); 
        
        builder.Property(x => x.DepartmentId)
            .HasConversion(d => d.Value, d => new DepartmentId(d))
            .HasColumnName("id");
        
        builder.Property(p => p.ParentId)
            .HasConversion(p => p!.Value, p => new DepartmentId(p))
            .HasColumnName("parent_id");

        builder.Property(n => n.DepartmentName)
            .HasConversion(n => n.Value, d => new DepartmentName(d))
            .HasColumnName("name");
        
        builder.Property(d => d.Identifier)
            .HasConversion(d => d.Value, d => new Identifier(d))
            .HasColumnName("identifier");
        
        builder.Property(d => d.Path)
            .HasConversion(d => d.Value, d => new Path(d))
            .HasColumnName("path");
        
        builder.Property(d => d.IsActive).HasColumnName("is_active");
        
        builder.Property(d => d.CreatedAt).HasColumnName("created_at");
        
        builder.Property(d => d.UpdatedAt).HasColumnName("updated_at");

        builder.HasMany(d => d.ChildrenDepartments)
            .WithOne()
            .IsRequired(false)
            .HasForeignKey(d => d.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(d => d.DepartmentLocations)
            .WithOne()
            .HasForeignKey(d => d.DepartmentId);
        
        builder
            .HasMany( d => d.DepartmentPositions)
            .WithOne()
            .HasForeignKey(dp => dp.DepartmentId);
        
        builder.HasIndex(d => d.Identifier).IsUnique().HasDatabaseName("ix_department_identifier");
    }
}
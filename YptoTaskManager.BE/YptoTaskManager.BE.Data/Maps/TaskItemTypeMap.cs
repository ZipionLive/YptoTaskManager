using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YptoTaskManager.BE.Domain.Entities;

namespace YptoTaskManager.BE.Data.Maps
{
    public class TaskItemTypeMap : IEntityTypeConfiguration<TaskItemType>
    {
        public void Configure(EntityTypeBuilder<TaskItemType> builder)
        {
            builder.ToTable("TaskItemTypes");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.CreatedOn)
                .HasDefaultValueSql("GETUTCDATE()")
                .IsRequired();

            builder.HasIndex(x => new { x.ParentId, x.Name }).IsUnique();

            builder.HasOne(x => x.CreatedBy)
                .WithMany()
                .HasForeignKey(x => x.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ModifiedBy)
                .WithMany()
                .HasForeignKey(x => x.ModifiedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.DeletedBy)
                .WithMany()
                .HasForeignKey(x => x.DeletedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Parent)
                .WithMany()
                .HasForeignKey(x => x.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

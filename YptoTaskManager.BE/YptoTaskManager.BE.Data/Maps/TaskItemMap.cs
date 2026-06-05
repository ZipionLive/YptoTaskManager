using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YptoTaskManager.BE.Domain.Entities;

namespace YptoTaskManager.BE.Data.Maps
{
    public class TaskItemMap : IEntityTypeConfiguration<TaskItem>
    {
        public void Configure(EntityTypeBuilder<TaskItem> builder)
        {
            builder.ToTable("Tasks");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.CreatedOn)
                .HasDefaultValueSql("GETUTCDATE()")
                .IsRequired();

            builder.HasOne(x => x.CreatedBy)
                .WithMany(x => x.CreatedTasks)
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

            builder.HasOne(x => x.TaskType)
                .WithMany()
                .HasForeignKey(x => x.TaskTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.TaskStatus)
                .WithMany()
                .HasForeignKey(x => x.TaskStatusId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

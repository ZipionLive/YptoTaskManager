using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YptoTaskManager.BE.Domain.Entities;

namespace YptoTaskManager.BE.Data.Maps
{
    public class TaskItemStatusMap : IEntityTypeConfiguration<TaskItemStatus>
    {
        public void Configure(EntityTypeBuilder<TaskItemStatus> builder)
        {
            builder.ToTable("TaskItemStatuses");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.HasIndex(x => x.Name).IsUnique();

            builder.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}

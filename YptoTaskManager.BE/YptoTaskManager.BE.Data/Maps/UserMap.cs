using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YptoTaskManager.BE.Domain.Entities;
using YptoTaskManager.BE.Domain.Enums;

namespace YptoTaskManager.BE.Data.Maps
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            builder.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
            builder.Property(x => x.LastName).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Email).HasMaxLength(320).IsRequired();
            builder.Property(x => x.PhoneNumber).HasMaxLength(50).IsRequired();
            builder.Property(x => x.PasswordSalt).IsRequired();
            builder.Property(x => x.PasswordHash).HasMaxLength(512).IsRequired();
            builder.Property(x => x.Role)
                .HasConversion<int>()
                .HasDefaultValue(UserRole.User)
                .IsRequired();

            builder.HasIndex(x => x.Email).IsUnique();

            builder.HasMany(x => x.CreatedTasks)
                .WithOne(x => x.CreatedBy)
                .HasForeignKey(x => x.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.AssignedTasks)
                .WithMany(x => x.AssignedTo)
                .UsingEntity<Dictionary<string, object>>(
                    "TaskItemAssignments",
                    right => right
                        .HasOne<TaskItem>()
                        .WithMany()
                        .HasForeignKey("TaskItemId"),
                    left => left
                        .HasOne<User>()
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade),
                    join =>
                    {
                        join.ToTable("TaskItemAssignments");
                        join.HasKey("TaskItemId", "UserId");
                    });
        }
    }
}

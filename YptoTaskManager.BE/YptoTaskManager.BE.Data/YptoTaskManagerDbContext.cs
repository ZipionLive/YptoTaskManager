using Microsoft.EntityFrameworkCore;
using YptoTaskManager.BE.Domain.Entities;

namespace YptoTaskManager.BE.Data
{
    public class YptoTaskManagerDbContext : DbContext
    {
        public YptoTaskManagerDbContext(DbContextOptions<YptoTaskManagerDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<TaskItem> TaskItems => Set<TaskItem>();
        public DbSet<TaskItemStatus> TaskItemStatuses => Set<TaskItemStatus>();
        public DbSet<TaskItemType> TaskItemTypes => Set<TaskItemType>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(YptoTaskManagerDbContext).Assembly);
        }
    }
}

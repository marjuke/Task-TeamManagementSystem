using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Persistance
{
    public class AppDBContext(DbContextOptions options) : IdentityDbContext<User>(options)
    {
        //public DbSet<Activity> Activities { get; set; }

        public DbSet<Status> Statuses => Set<Status>();
        public DbSet<Team> Teams => Set<Team>();
        public DbSet<WorkTask> WorkTasks => Set<WorkTask>();
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<WorkTask>(e =>
            {
                e.HasOne(x => x.Status)
                 .WithMany(s => s.Tasks)
                 .HasForeignKey(x => x.StatusID)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.Team)
                 .WithMany(t => t.Tasks)
                 .HasForeignKey(x => x.TeamId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.AssignedToUser)
                 .WithMany()
                 .HasForeignKey(x => x.AssignedToUserID)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.CreatedByUser)
                 .WithMany()
                 .HasForeignKey(x => x.CreatedByUserID)
                 .OnDelete(DeleteBehavior.Restrict);
            });
        }

    }
}

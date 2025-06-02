using Lineupper.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lineupper.Infrastructure
{
    public class LineupperDbContext : DbContext
    {
        public LineupperDbContext(DbContextOptions<LineupperDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Organizer> Organizers => Set<Organizer>();
        public DbSet<Participant> Participants => Set<Participant>();
        public DbSet<Festival> Festivals => Set<Festival>();
        public DbSet<Band> Bands => Set<Band>();
        public DbSet<Vote> Votes => Set<Vote>();
        public DbSet<ScheduleItem> ScheduleItems => Set<ScheduleItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasDiscriminator<string>("Role")
                .HasValue<Organizer>("Organizer")
                .HasValue<Participant>("Participant");

            modelBuilder.Entity<Festival>()
                .HasOne(f => f.Organizer)
                .WithMany(o => o.Festivals)
                .HasForeignKey(f => f.OrganizerId);

            modelBuilder.Entity<Vote>()
                .HasOne(v => v.Participant)
                .WithMany(p => p.Votes)
                .HasForeignKey(v => v.ParticipantId);
        }
    }
}
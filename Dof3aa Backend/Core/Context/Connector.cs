
using Core.entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Context
{
    public class Connector : IdentityDbContext<AppUser>
    {
        public Connector(DbContextOptions<Connector> options) : base(options)
        {

        }
        public DbSet<Cource> Cources { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<task> Tasks { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Topic> Topics { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AppUser>().HasKey(u => u.UserName);
            modelBuilder.Entity<UserGroup>().HasKey(ug => new { ug.Username, ug.CourceId });
            modelBuilder.Entity<UserGroup>().HasOne(ug => ug.User).WithMany(u => u.UserGroups).HasForeignKey(ug => ug.Username);
            modelBuilder.Entity<UserGroup>().HasOne(ug => ug.Cource).WithMany(c => c.UserGroups).HasForeignKey(ug => ug.CourceId);
            modelBuilder.Entity<UserNotification>().HasKey(un => new { un.NotificationId, un.ReceiverUserName });
        }
    }
}

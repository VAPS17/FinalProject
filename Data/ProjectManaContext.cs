using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using FinalProject.Models;
using System.Linq;

namespace FinalProject.Data
{
    public class ProjectManaContext : DbContext
    {
        public ProjectManaContext(DbContextOptions<ProjectManaContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MemberProject>().HasKey(mp => new { mp.MemberId, mp.ProjectId});
            modelBuilder.Entity<MemberProject>()
                .HasOne(mp => mp.Member)
                .WithMany(mp => mp.MemberProjects)
                .HasForeignKey(mp => mp.MemberId);
            modelBuilder.Entity<MemberProject>()
                .HasOne(mp => mp.Project)
                .WithMany(mp => mp.ProjectMembers)
                .HasForeignKey(mp => mp.ProjectId);

            var foreignKeysWithCascadeDelete = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);
            foreach (var fk in foreignKeysWithCascadeDelete)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        public DbSet<FinalProject.Models.P_Task> P_Task { get; set; }

        public DbSet<FinalProject.Models.Member> Member { get; set; }

        public DbSet<FinalProject.Models.Meeting> Meeting { get; set; }

        public DbSet<FinalProject.Models.Project> Project { get; set; }

        public DbSet<FinalProject.Models.MemberProject> MemberProject { get; set; }

        public DbSet<FinalProject.Models.State> State{ get; set; }

        public DbSet<FinalProject.Models.Function> Function { get; set; }
    }
}

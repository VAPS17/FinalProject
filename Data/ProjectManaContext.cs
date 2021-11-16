using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using FinalProject.Models;

namespace FinalProject.Data
{
    public class ProjectManaContext : DbContext
    {
        public ProjectManaContext(DbContextOptions<ProjectManaContext> options)
            : base(options)
        {
        }

        public DbSet<FinalProject.Models.P_Task> P_Task { get; set; }

        public DbSet<FinalProject.Models.Meeting> Meeting { get; set; }
    }
}

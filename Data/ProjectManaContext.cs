using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject.Data
{
    public class ProjectManaContext : DbContext
    {
        public ProjectManaContext(DbContextOptions<ProjectManaContext> options)
            : base(options)
        {
        }

        public DbSet<FinalProject.Models.Task> Task { get; set; }
    }
}

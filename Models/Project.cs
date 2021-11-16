using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class Project
    {
        public int ProjectId { get; set; }

        [Required]
        [StringLength(50)]
        public string ProjectName { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime FinishDate { get; set; }

        public string Annotations { get; set; }

        public bool ProjectState { get; set; }

        public ICollection<Project> Projects { get; set; }
    }

}

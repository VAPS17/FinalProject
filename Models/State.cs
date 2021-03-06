using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class State
    {
        public int StateId { get; set; }

        [Required]
        [StringLength(20)]
        public string StateValue { get; set; }

        public ICollection<P_Task> P_Task { get; set; }

        public ICollection<Project> Projects { get; set; }
    }
}

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
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        [StringLength(50)]
        public string ProjectCreator { get; set; }

        public string NumberEmployees { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime FinishDate { get; set; }

        [Required]
        public DateTime DecisiveDeliveryDate { get; set; } //prazo

        public ICollection<P_Task> P_Task { get; set; }

        public ICollection<Meeting> Meeting { get; set; }

        public ICollection<MemberProject> ProjectMembers { get; set; }
    }

}

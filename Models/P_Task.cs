using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class P_Task
    {
        public int P_TaskId { get; set; }

        [Required]
        [StringLength(200)]
        public string P_TaskName { get; set; }

        [StringLength(500)]
        public string Comentary { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        public DateTime StartDate { get; set; }
        
        [Required]
        public DateTime Deadline { get; set; }

        public DateTime EffectiveEndDate { get; set; }
        
        [DisplayName("Member")]
        public int MemberId { get; set; }
        public Member Member { get; set; }
        
        [DisplayName("State")]
        public int StateId { get; set; }
        public State State { get; set; }

        [DisplayName("Project")]
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }

}

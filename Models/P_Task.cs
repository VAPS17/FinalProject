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
        [StringLength(512)]
        public string P_TaskName { get; set; }

        public string Comentary { get; set; }

        public string P_TaskState { get; set; }

        [DisplayName("Project")]
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }

}

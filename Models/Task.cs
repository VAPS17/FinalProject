using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class Task
    {
        public int TaskId { get; set; }

        [Required]
        [StringLength(512)]
        public string TaskName { get; set; }

        public string Comentary { get; set; }

        public string TaskState { get; set; }
    }

}

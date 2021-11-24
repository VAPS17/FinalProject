using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class Meeting
    {
        public int MeetingId { get; set; }

        [Required]
        public DateTime DateandTime { get; set; }

        [Required]
        [StringLength(200)]
        public string Topic { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}

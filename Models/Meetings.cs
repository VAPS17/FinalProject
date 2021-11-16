using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class Meetings
    {
        public int MeetingId { get; set; }

        [Required]
        public DateTime MeetingDateandTime { get; set; }

        [Required]
        [StringLength(300)]
        public string MeetingTopic { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class Member
    {
        public int MemberId { get; set; }
        [Required]
        [StringLength(256)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(6)]
        public int EmployeeNumber { get; set; }
        public bool Manager { get; set; }

        [DisplayName("Project")]
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}

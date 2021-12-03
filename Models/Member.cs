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
        [DisplayName("Employee Number")]
        public string EmployeeNumber { get; set; }

        public ICollection<MemberProject> MemberProjects { get; set; }

        [DisplayName("Function")]
        public int FunctionId { get; set; }
        public Function Function { get; set; }

    }
}

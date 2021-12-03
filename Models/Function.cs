using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class Function
    {

        public int FunctionId { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        public ICollection<Member> Members { get; set; }

    }
}

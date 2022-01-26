using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using FinalProject.Models;

namespace FinalProject.ViewModels
{
    public class CreateMemberViewModel
    {
        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(6)]
        [DisplayName("Employee Number")]
        public string EmployeeNumber { get; set; }

        [DisplayName("Function")]
        public int FunctionId { get; set; }
        public Function Function { get; set; }

        [DisplayName("Create a Normal Member or a Project Manager?")]
        public string Role { get; set; }

        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }

    }
}

using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.ViewModels
{
    public class MemberListViewModel
    {
        public IEnumerable<Member> Members { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}

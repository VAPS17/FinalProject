using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.ViewModels
{
    public class P_TaskListViewModel
    {
        public IEnumerable<P_Task> P_Task { get; set; }
        public PagingInfo P_TaskPagingInfo { get; set; }
    }
}

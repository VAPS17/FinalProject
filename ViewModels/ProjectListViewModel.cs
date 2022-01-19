using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.ViewModels
{
    public class ProjectListViewModel
    {
        public Project Project { get; set; }

        public IEnumerable<Project> Projects { get; set; }

        public IEnumerable<P_Task> P_Task { get; set; }

        public PagingInfo Pagination { get; set; }

        public string ProjectNameSearched { get; set; }
    }
}

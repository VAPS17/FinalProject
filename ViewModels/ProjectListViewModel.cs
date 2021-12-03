﻿using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.ViewModels
{
    public class ProjectListViewModel
    {
        public IEnumerable<Project> Projects { get; set; }

        public PagingInfo PagingInfo { get; set; }

        public string ProjectNameSearched { get; set; }
    }
}
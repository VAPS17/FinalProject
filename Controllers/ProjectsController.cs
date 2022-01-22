using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalProject.Data;
using FinalProject.Models;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Http;

namespace FinalProject.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ProjectManaContext _context;

        public ProjectsController(ProjectManaContext context)
        {
            _context = context;
        }

        // GET: Projects
        public async Task<IActionResult> Index(string search, int page = 1)
        {
            var projectsSearch = _context.Project
                .Where(b => search == null || b.Name.Contains(search));

            var pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                TotalItems = projectsSearch.Count()
            };

            if (pagingInfo.CurrentPage > pagingInfo.TotalPages)
            {
                pagingInfo.CurrentPage = pagingInfo.TotalPages;
            }

            if (pagingInfo.CurrentPage < 1)
            {
                pagingInfo.CurrentPage = 1;
            }

            var projects = await projectsSearch
                            .OrderBy(b => b.Name)
                            .Skip((pagingInfo.CurrentPage - 1) * pagingInfo.PageSize)
                            .Take(pagingInfo.PageSize)
                            .ToListAsync();

            return View(
                new ProjectListViewModel
                {
                    Projects = projects,
                    Pagination = pagingInfo,
                    ProjectNameSearched = search
                }
            );
        }


        // GET: Projects/Details/5
        public async Task<IActionResult> Details(IFormCollection frm, int? id, int page = 1)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .FirstOrDefaultAsync(p => p.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }

            string stateRadio = frm["State"].ToString();

            ViewData["TerminationValidation"] = TerminationValidation(id);
            ViewData["NotStartedExist"] = _context.P_Task.Any(e => e.StateId == 1 && e.ProjectId == id);
            ViewData["InProgressExist"] = _context.P_Task.Any(e => e.StateId == 2 && e.ProjectId == id);
            ViewData["FinishedExist"] = _context.P_Task.Any(e => e.StateId == 3 && e.ProjectId == id);
            ViewData["DeadlineNotStarted"] = TaskValidation(id, 1);
            ViewData["DeadlineInProgress"] = TaskValidation(id, 2);
            ViewData["CurrentState"] = stateRadio;
            ViewData["Delayed"] = false;

            if (stateRadio == "Delayed")
            {
                stateRadio = "";
                ViewData["Delayed"] = true;
            }

            var P_TaskSearch = _context.P_Task
                                .Where(x => x.State.StateValue == stateRadio || stateRadio == "")
                                .Where(t => t.ProjectId == id)
                                .Include(b => b.Project)
                                .Include(b => b.State);


            

            var p_task = await P_TaskSearch
                            .OrderBy(b => b.CreationDate)
                            .ToListAsync();

            return View(
                new ProjectListViewModel
                {
                    P_Task = p_task,
                    Project = project
                }
            );
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectId,Name,Description,ProjectCreator,StartDate,FinishDate,DecisiveDeliveryDate")] Project project)
        {
            if (project.StartDate >= project.DecisiveDeliveryDate)
            {
                ModelState.AddModelError("DecisiveDeliveryDate", "Start date is high than Decisive delivery date");
            }

            if (ModelState.IsValid)
            {
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectId,Name,Description,ProjectCreatorId,StartDate,FinishDate,DecisiveDeliveryDate")] Project project)
        {


            if (id != project.ProjectId)
            {
                return NotFound();
            }

            if (project.StartDate >= project.DecisiveDeliveryDate)
            {
                ModelState.AddModelError("DecisiveDeliveryDate", "Start Date is high than Decisive delivery date");
            }

            if (ModelState.IsValid)
            {

                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.ProjectId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Project.FindAsync(id);
            _context.Project.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET
        public async Task<IActionResult> AddMember(int id)
        {
            var members = await _context.Member
                            .Include(m => m.Function)
                            .Include(m => m.MemberProjects)
                            .OrderBy(m => m.Name)
                            .ToListAsync();

            foreach (var member in members.ToList())
            {
                if (member.MemberProjects.Count() != 0)
                {
                    foreach (var memberProject in member.MemberProjects)
                    {
                        if (member.MemberId == memberProject.MemberId && id == memberProject.ProjectId)
                        {
                            members.Remove(member);
                        }
                    }
                }
            }

            return View(
                new MemberListViewModel
                {
                    Members = members
                }
            );
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMember(IFormCollection frm, int id, [Bind("MemberId,ProjectId")] MemberProject memberProject) {

            string buttonId= frm["MemberProject"].ToString();

            int IdMember = Convert.ToInt32(buttonId);

            if (ModelState.IsValid)
            {
                memberProject.MemberId = IdMember;
                memberProject.ProjectId = id;
                _context.Add(memberProject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(AddMember));
            }
            return View(memberProject);
        }

        private bool TaskValidation(int? id, int Stateid)
        {
            var numDelayedDates = _context.P_Task.Where(a => a.ProjectId == id && System.DateTime.Now.Date > a.Deadline && a.StateId == Stateid).Count();
            var totalDeadlineDates = _context.P_Task.Where(a => a.ProjectId == id && a.StateId == Stateid).Count();

            if (numDelayedDates >= 0 && totalDeadlineDates == 0)
            {
                return false;
            }
            else if (numDelayedDates == totalDeadlineDates)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool TerminationValidation(int? id)
        {
            var numOfDelyedDates = _context.P_Task.Where(a => a.ProjectId == id && System.DateTime.Now.Date > a.Deadline).Count();
            var finishedState = _context.P_Task.Where(b => b.ProjectId == id && System.DateTime.Now.Date > b.Deadline && b.StateId == 3).Count();

            if (numOfDelyedDates == finishedState)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool ProjectExists(int id)
        {
            return _context.Project.Any(e => e.ProjectId == id);
        }
    }
}

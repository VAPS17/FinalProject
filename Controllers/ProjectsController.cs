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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace FinalProject.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ProjectManaContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public ProjectsController(ProjectManaContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Authorize(Roles = "manager,member,admin")]
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

            string email = User.Identity.Name;
            bool checkManager = User.IsInRole("manager");
            bool checkMember = User.IsInRole("member");

            if (checkManager)
            {
                var memberId = _context.Member.Where(m => m.Email.Equals(email)).Select(m => m.MemberId).First();

                var projects = await projectsSearch
                .Where(p => p.ProjectCreatorId == memberId)
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
            else if (checkMember)
            {
                var memberId = _context.Member.Where(m => m.Email.Equals(email)).Select(m => m.MemberId).First();

                var memberProjectId = _context.MemberProject.Where(p => p.MemberId == memberId).Select(p => p.ProjectId);

                var projectsId = _context.Project.Select(p => p.ProjectId);

                var IdComparation = projectsId.Except(memberProjectId);

                var projects = await projectsSearch
                            .OrderBy(b => b.Name)
                            .Skip((pagingInfo.CurrentPage - 1) * pagingInfo.PageSize)
                            .Take(pagingInfo.PageSize)
                            .ToListAsync();

                foreach (var project in projects.ToList())
                {
                    foreach (var projectId in IdComparation)
                    {
                        if (project.ProjectId == projectId)
                        {
                            projects.Remove(project);
                        }
                    }
                }

                return View(
                    new ProjectListViewModel
                    {
                        Projects = projects,
                        Pagination = pagingInfo,
                        ProjectNameSearched = search
                    }
                );

            }

            if (User.IsInRole("admin"))
            {
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
                    });
            }

            return View();
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

            var mettings = _context.Meeting;

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
                    Project = project,
                    Meetings = mettings
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
        [Authorize(Roles = "manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectId,Name,Description,ProjectCreator,StartDate,FinishDate,DecisiveDeliveryDate")] Project project)
        {

            project.StartDate = DateTime.Now;

            if (project.StartDate >= project.DecisiveDeliveryDate)
            {
                ModelState.AddModelError("DecisiveDeliveryDate", "Decisive delivery date is higher than system date");
            }

            if (ModelState.IsValid)
            {
                string currentLogin = User.Identity.Name;

                project.ProjectCreatorId = _context.Member.Where(m => m.Email.Equals(currentLogin)).Select(m => m.MemberId).First();

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

            project.StartDate = DateTime.Now;

            if (project.StartDate >= project.DecisiveDeliveryDate)
            {
                ModelState.AddModelError("DecisiveDeliveryDate", "Decisive delivery date is higher than system date");
            }

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

        // GET AddMember
        public async Task<IActionResult> AddMember(string search, int id, int page = 1)
        {
            ViewData["ID"] = id;

            var memberSearch = _context.Member
                .Where(b => search == null || b.Function.Name.Contains(search) || b.EmployeeNumber.Contains(search) || b.Email.Contains(search) || b.PhoneNumber.Contains(search));

            var pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                TotalItems = memberSearch.Count()
            };

            if (pagingInfo.CurrentPage > pagingInfo.TotalPages)
            {
                pagingInfo.CurrentPage = pagingInfo.TotalPages;
            }

            if (pagingInfo.CurrentPage < 1)
            {
                pagingInfo.CurrentPage = 1;
            }

            var members = await memberSearch
                            .Include(m => m.Function)
                            .Include(m => m.MemberProjects)
                            .OrderBy(m => m.Name)
                            .Skip((pagingInfo.CurrentPage - 1) * pagingInfo.PageSize)
                            .Take(pagingInfo.PageSize)
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
                    Members = members,
                    PagingInfo = pagingInfo
                }
            );
        }

        // POST AddMember
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMember(IFormCollection frm, int id, [Bind("MemberId,ProjectId")] MemberProject memberProject)
        {

            string buttonId = frm["AddMemberProject"].ToString();

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

        //GET Remove Member
        public async Task<IActionResult> RemoveMember(int id, string search, int page = 1)
        {
            ViewData["ID"] = id;

            var memberSearch = _context.Member
                .Where(b => search == null || b.Function.Name.Contains(search) || b.EmployeeNumber.Contains(search) || b.Email.Contains(search) || b.PhoneNumber.Contains(search));

            var memberIds = _context.Member.Select(a => a.MemberId);

            var memberProjectsIds = _context.MemberProject.Where(a => a.ProjectId == id).Select(b => b.MemberId);

            var IdComparation = memberIds.Except(memberProjectsIds);

            var pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                TotalItems = memberProjectsIds.Count()
            };

            if (pagingInfo.CurrentPage > pagingInfo.TotalPages)
            {
                pagingInfo.CurrentPage = pagingInfo.TotalPages;
            }

            if (pagingInfo.CurrentPage < 1)
            {
                pagingInfo.CurrentPage = 1;
            }

            var members = await memberSearch
                            .Include(m => m.Function)
                            .Include(m => m.MemberProjects)
                            .OrderBy(m => m.Name)
                            .Skip((pagingInfo.CurrentPage - 1) * pagingInfo.PageSize)
                            .Take(pagingInfo.PageSize)
                            .ToListAsync();

            foreach (var member in members.ToList())
            {
                foreach (var memberId in IdComparation)
                {
                    if (member.MemberId == memberId)
                    {
                        members.Remove(member);
                    }
                }
            }

            if (members.Count() == 0)
            {
                ViewData["member"] = false;
            }
            else
            {
                ViewData["member"] = true;
            }

            return View(
                new MemberListViewModel
                {
                    Members = members,
                    PagingInfo = pagingInfo
                }
            );
        }

        // POST RemoveMember
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveMember(IFormCollection frm, int id)
        {

            string buttonId = frm["RemoveMemberProject"].ToString();

            int IdMember = Convert.ToInt32(buttonId);

            var member = await _context.MemberProject.FindAsync(IdMember, id);
            _context.MemberProject.Remove(member);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(RemoveMember));
        }

        //GET AssignMember
        public async Task<IActionResult> AssignMember(int id, string search, int page = 1)
        {
            ViewData["ID"] = id;

            var memberSearch = _context.Member
                .Where(b => search == null || b.Function.Name.Contains(search) || b.EmployeeNumber.Contains(search) || b.Email.Contains(search) || b.PhoneNumber.Contains(search));

            var memberIds = _context.Member.Select(a => a.MemberId);

            var memberProjectsIds = _context.MemberProject.Where(a => a.ProjectId == id).Select(b => b.MemberId);

            var IdComparation = memberIds.Except(memberProjectsIds);

            var pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                TotalItems = memberProjectsIds.Count()
            };

            if (pagingInfo.CurrentPage > pagingInfo.TotalPages)
            {
                pagingInfo.CurrentPage = pagingInfo.TotalPages;
            }

            if (pagingInfo.CurrentPage < 1)
            {
                pagingInfo.CurrentPage = 1;
            }

            var members = await memberSearch
                            .Include(m => m.Function)
                            .Include(m => m.MemberProjects)
                            .OrderBy(m => m.Name)
                            .Skip((pagingInfo.CurrentPage - 1) * pagingInfo.PageSize)
                            .Take(pagingInfo.PageSize)
                            .ToListAsync();

            foreach (var member in members.ToList())
            {
                foreach (var memberId in IdComparation)
                {
                    if (member.MemberId == memberId)
                    {
                        members.Remove(member);
                    }
                }
            }

            if (members.Count() == 0)
            {
                ViewData["member"] = false;
            }
            else
            {
                ViewData["member"] = true;
            }

            return View(
                new MemberListViewModel
                {
                    Members = members,
                    PagingInfo = pagingInfo
                }
            );
        }

        // POST AssignMember
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignMember(IFormCollection frm, int id)
        {

            string buttonId = frm["AssignMemberProject"].ToString();

            int IdMember = Convert.ToInt32(buttonId);

            var member = await _context.MemberProject.FindAsync(IdMember, id);
            _context.MemberProject.Remove(member);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(RemoveMember));
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
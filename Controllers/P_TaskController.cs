using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalProject.Data;
using FinalProject.Models;
using FinalProject.ViewModels;
using System.Data;
using Microsoft.AspNetCore.Http;
using System;

namespace FinalProject.Controllers
{
    public class P_TaskController : Controller
    {
        private readonly ProjectManaContext _context;
        public P_TaskController(ProjectManaContext context)
        {
            _context = context;
        }

        // GET: P_Task/Create
        public IActionResult Create(int id)
        {
            ViewBag.ID = id;
            return View();
        }

        // POST: P_Task/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("P_TaskId,P_TaskName,Comentary," +
            ",CreationDate,StartDate,Deadline,EffectiveEndDate,ProjectId,StateId,MemberId")] P_Task p_task)
        {
            if (p_task.Deadline <= System.DateTime.Now.Date)
            {
                ModelState.AddModelError("Deadline", "Deadline can't be lesse then the CURRENT DATE.");
            }

            if (ModelState.IsValid)
            {
                p_task.ProjectId = id;
                p_task.StateId = 1;
                p_task.CreationDate = System.DateTime.Now.Date;
                p_task.MemberId = _context.Project.Where(p => p.ProjectId == id).Select(p => p.ProjectCreatorId).First();

                _context.Add(p_task);
                await _context.SaveChangesAsync();

                ViewBag.Title = "Task Creation";
                ViewBag.Message = "Task sucessfully created.";
                ViewBag.ID = id;

                return View("Success");
            }

            ViewBag.ID = id;
            
            ViewData["ProjectId"] = new SelectList(_context.Set<Project>(), "ProjectId", "Name", p_task.ProjectId);
            ViewData["StateId"] = new SelectList(_context.Set<State>(), "StateId", "StateValue", p_task.StateId);
            return View(p_task);
        }

        // GET: P_Task/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var p_task = await _context.P_Task.FindAsync(id);
            if (p_task == null)
            {
                return NotFound();
            }
            ViewData["ProjectId"] = new SelectList(_context.Set<Project>(), "ProjectId", "Name", p_task.ProjectId);
            ViewData["StateId"] = new SelectList(_context.Set<State>(), "StateId", "StateValue", p_task.StateId);
            return View(p_task);
        }

        // POST: P_Task/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("P_TaskId,P_TaskName," +
            ",CreationDate,StartDate,Deadline,EffectiveEndDate,Comentary,ProjectId,StateId,MemberId")] P_Task p_task)
        {
            if (id != p_task.P_TaskId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(p_task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!P_TaskExists(p_task.P_TaskId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                ViewBag.Title = "Task Edition";
                ViewBag.Message = "Task successfully altered.";
                ViewBag.ID = p_task.ProjectId;
                return View("Success");
            }
            ViewData["ProjectId"] = new SelectList(_context.Set<Project>(), "ProjectId", "Name", p_task.ProjectId);
            ViewData["StateId"] = new SelectList(_context.Set<State>(), "StateId", "StateValue", p_task.StateId);
            return View(p_task);
        }

        // GET: P_Task/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var p_task = await _context.P_Task
                .Include(b => b.Project)
                .Include(b => b.State)
                .FirstOrDefaultAsync(m => m.P_TaskId == id);
            if (p_task == null)
            {
                return NotFound();
            }

            return View(p_task);
        }

        // POST: P_Task/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var p_task = await _context.P_Task.FindAsync(id);
            _context.P_Task.Remove(p_task);
            await _context.SaveChangesAsync();

            ViewBag.ID = p_task.ProjectId;
            ViewBag.Title = "Task Elimination.";
            ViewBag.Message = "Task sucessfully deleted.";
            return View("Success");
        }

        //GET EditTaskInProgress
        public async Task<IActionResult> EditTaskInProgress(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var p_task = await _context.P_Task.FindAsync(id);
            if (p_task == null)
            {
                return NotFound();
            }
            ViewData["ProjectId"] = new SelectList(_context.Set<Project>(), "ProjectId", "Name", p_task.ProjectId);
            ViewData["StateId"] = new SelectList(_context.Set<State>(), "StateId", "StateValue", p_task.StateId);
            return View(p_task);
        }

        //POST EditTaskInProgress
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTaskInProgress(int id, [Bind("P_TaskId,P_TaskName," +
            ",CreationDate,StartDate,Deadline,EffectiveEndDate,Comentary,ProjectId,StateId,MemberId")] P_Task p_task)
        {
            if (id != p_task.P_TaskId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    p_task.StartDate = System.DateTime.Now.Date;
                    p_task.StateId = 2;
                    _context.Update(p_task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!P_TaskExists(p_task.P_TaskId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                ViewBag.Title = "Progress Edtion.";
                ViewBag.Message = "Progress change was a success.";
                ViewBag.ID = p_task.ProjectId;
                return View("Success");
            }
            ViewData["ProjectId"] = new SelectList(_context.Set<Project>(), "ProjectId", "Name", p_task.ProjectId);
            ViewData["StateId"] = new SelectList(_context.Set<State>(), "StateId", "StateValue", p_task.StateId);
            return View(p_task);
        }

        //GET EditTaskTerminate
        public async Task<IActionResult> EditTaskTerminate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var p_task = await _context.P_Task.FindAsync(id);
            if (p_task == null)
            {
                return NotFound();
            }
            ViewData["ProjectId"] = new SelectList(_context.Set<Project>(), "ProjectId", "Name", p_task.ProjectId);
            ViewData["StateId"] = new SelectList(_context.Set<State>(), "StateId", "StateValue", p_task.StateId);
            return View(p_task);
        }

        //POST EditTaskTerminate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTaskTerminate(int id, [Bind("P_TaskId,P_TaskName," +
            ",CreationDate,StartDate,Deadline,EffectiveEndDate,Comentary,ProjectId,StateId,MemberId")] P_Task p_task)
        {
            if (id != p_task.P_TaskId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    p_task.EffectiveEndDate = System.DateTime.Now.Date;
                    p_task.StateId = 3;
                    _context.Update(p_task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!P_TaskExists(p_task.P_TaskId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                ViewBag.Title = "Progress Edtion.";
                ViewBag.Message = "Progress change was a success.";
                ViewBag.ID = p_task.ProjectId;
                return View("Success");
            }
            ViewData["ProjectId"] = new SelectList(_context.Set<Project>(), "ProjectId", "Name", p_task.ProjectId);
            ViewData["StateId"] = new SelectList(_context.Set<State>(), "StateId", "StateValue", p_task.StateId);
            return View(p_task);
        }

        //GET EditComentary
        public async Task<IActionResult> EditComentary(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var p_task = await _context.P_Task.FindAsync(id);
            if (p_task == null)
            {
                return NotFound();
            }
            ViewData["ProjectId"] = new SelectList(_context.Set<Project>(), "ProjectId", "Name", p_task.ProjectId);
            ViewData["StateId"] = new SelectList(_context.Set<State>(), "StateId", "StateValue", p_task.StateId);
            return View(p_task);
        }

        //POST EditComentary
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditComentary(int id, [Bind("P_TaskId,P_TaskName," +
            ",CreationDate,StartDate,Deadline,EffectiveEndDate,Comentary,ProjectId,StateId,MemberId")] P_Task p_task)
        {
            if (id != p_task.P_TaskId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(p_task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!P_TaskExists(p_task.P_TaskId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                ViewBag.Title = "Comment Saved";
                ViewBag.ID = p_task.ProjectId;
                return View("Success");
            }
            ViewData["ProjectId"] = new SelectList(_context.Set<Project>(), "ProjectId", "Name", p_task.ProjectId);
            ViewData["StateId"] = new SelectList(_context.Set<State>(), "StateId", "StateValue", p_task.StateId);
            return View(p_task);
        }

        //GET ReverseProgretion
        public async Task<IActionResult> ReverseProgretion(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var p_task = await _context.P_Task.FindAsync(id);
            if (p_task == null)
            {
                return NotFound();
            }
            ViewData["ProjectId"] = new SelectList(_context.Set<Project>(), "ProjectId", "Name", p_task.ProjectId);
            ViewData["StateId"] = new SelectList(_context.Set<State>(), "StateId", "StateValue", p_task.StateId);
            return View(p_task);
        }

        //POST ReverseProgretion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReverseProgretion(int id, [Bind("P_TaskId,P_TaskName," +
            ",CreationDate,StartDate,Deadline,EffectiveEndDate,Comentary,ProjectId,StateId,MemberId")] P_Task p_task)
        {
            int num = p_task.StateId;

            if (id != p_task.P_TaskId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    p_task.StateId = p_task.StateId - 1;
                    _context.Update(p_task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!P_TaskExists(p_task.P_TaskId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                ViewBag.Title = "Reverse Progretion.";
                ViewBag.Message = "Progretion Saved.";
                ViewBag.ID = p_task.ProjectId;
                return View("Success");
            }
            ViewData["ProjectId"] = new SelectList(_context.Set<Project>(), "ProjectId", "Name", p_task.ProjectId);
            ViewData["StateId"] = new SelectList(_context.Set<State>(), "StateId", "StateValue", p_task.StateId);
            return View(p_task);
        }

        //GET AssignMember
        public async Task<IActionResult> AssignMember(int id, string search, int page = 1)
        {
            var projectId = _context.P_Task.Where(t => t.P_TaskId == id).Select(t => t.ProjectId).First();

            ViewData["ID"] = projectId;
            ViewData["creatorId"] = _context.Project.Where(p => p.ProjectId == projectId).Select(p => p.ProjectCreatorId).First();

            var memberSearch = _context.Member
                .Where(b => search == null || b.Function.Name.Contains(search) || b.EmployeeNumber.Contains(search) || b.Email.Contains(search) || b.PhoneNumber.Contains(search));

            var memberIds = _context.Member.Select(a => a.MemberId);

            var memberProjectsIds = _context.MemberProject.Where(a => a.ProjectId == projectId).Select(b => b.MemberId);

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
            var p_task = _context.P_Task.Find(id);

            string buttonId = frm["AssignMemberProject"].ToString();

            int IdMember = Convert.ToInt32(buttonId);

            if (ModelState.IsValid)
            {
                try
                {
                    p_task.MemberId = IdMember;
                    _context.Update(p_task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!P_TaskExists(p_task.P_TaskId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                ViewBag.Title = "Association of a User to a Task";
                ViewBag.Message = "Task successfully Unassign.";
                ViewBag.ID = p_task.ProjectId;
                return View("Success");
            }
            return View(p_task);
        }

        // POST UnassignMember
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnassignMember(IFormCollection frm, int id)
        {
            string buttonId = frm["UnassignMember"].ToString();

            int IdTask = Convert.ToInt32(buttonId);

            var p_task = _context.P_Task.Find(IdTask);

            if (ModelState.IsValid)
            {
                try
                {
                    p_task.MemberId = _context.Project.Where(p => p.ProjectId == id).Select(c => c.ProjectCreatorId).First();
                    _context.Update(p_task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!P_TaskExists(p_task.P_TaskId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                ViewBag.Title = "Association of a User to a Task";
                ViewBag.Message = "Task successfully associated.";
                ViewBag.ID = p_task.ProjectId;
                return View("Success");
            }
            return View(p_task);
        }

        private bool TaskValidation(int id, int Stateid)
        {
            var numDelayedDates = _context.P_Task.Where(a => a.ProjectId == id && System.DateTime.Now.Date > a.Deadline && a.StateId == Stateid).Count();
            var totalDeadlineDates = _context.P_Task.Where(a => a.ProjectId == id && a.StateId == Stateid).Count();

            if(numDelayedDates >= 0 && totalDeadlineDates == 0 )
            {
                return false;
            } else if (numDelayedDates == totalDeadlineDates)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool TerminationValidation(int id)
        {
            var numOfDelyedDates = _context.P_Task.Where(a => a.ProjectId == id && System.DateTime.Now.Date > a.Deadline).Count();
            var finishedState = _context.P_Task.Where(b => b.ProjectId == id && System.DateTime.Now.Date > b.Deadline && b.StateId == 3).Count();

            if (numOfDelyedDates == finishedState)
            {
                return false;
            } else
            {
                return true;
            }
        }

        private bool P_TaskExists(int id)
        {
            return _context.P_Task.Any(e => e.P_TaskId == id);
        }
    }
}

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
            ViewData["ProjectId"] = new SelectList(_context.Set<Project>(), "ProjectId", "Name", id);
            ViewData["StateId"] = new SelectList(_context.Set<State>(), "StateId", "StateValue");
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
                p_task.CreationDate = System.DateTime.Now.Date;

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

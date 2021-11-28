using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalProject.Data;
using FinalProject.Models;

namespace FinalProject.Controllers
{
    public class P_TaskController : Controller
    {
        private readonly ProjectManaContext _context;

        public P_TaskController(ProjectManaContext context)
        {
            _context = context;
        }

        // GET: P_Task
        public async Task<IActionResult> Index(int id)
        {
            var P_TasksContext = _context.P_Task.Where(t => t.ProjectId == id).Include(b => b.Project);
            return View(await P_TasksContext.ToListAsync());
        }

        // GET: P_Task/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var p_task = await _context.P_Task
                .Include(b => b.Project)
                .SingleOrDefaultAsync(b => b.ProjectId == id);
            if (p_task == null)
            {
                return NotFound();
            }

            return View(p_task);
        }

        // GET: P_Task/Create
        public IActionResult Create()
        {
            ViewData["ProjectId"] = new SelectList(_context.Set<Project>(), "ProjectId", "Name");
            return View();
        }

        // POST: P_Task/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("P_TaskId,P_TaskName,Comentary,P_TaskState,ProjectId")] P_Task p_task)
        {
            if (ModelState.IsValid)
            {
                _context.Add(p_task);
                await _context.SaveChangesAsync();

                ViewBag.Title = "Task added";
                ViewBag.Message = "Task sucessfully added.";
                return View("Success");
            }
            ViewData["ProjectId"] = new SelectList(_context.Set<Project>(), "ProjectId", "Name", p_task.ProjectId);
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
            return View(p_task);
        }

        // POST: P_Task/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("P_TaskId,P_TaskName,Comentary,P_TaskState,ProjectId")] P_Task p_task)
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

                ViewBag.Title = "Task edited";
                ViewBag.Message = "Taskessfully altered.";
                return View("Success");
            }
            ViewData["ProjectId"] = new SelectList(_context.Set<Project>(), "ProjectId", "Name", p_task.ProjectId);
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
                .FirstOrDefaultAsync(m => m.ProjectId == id);
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

            ViewBag.Title = "P_Task deleted";
            ViewBag.Message = "P_Task sucessfully deleted.";
            return View("Success");
        }

        private bool P_TaskExists(int id)
        {
            return _context.P_Task.Any(e => e.P_TaskId == id);
        }
    }
}

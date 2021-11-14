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
        public async Task<IActionResult> Index()
        {
            return View(await _context.P_Task.ToListAsync());
        }

        // GET: P_Task/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var p_Task = await _context.P_Task
                .FirstOrDefaultAsync(m => m.P_TaskId == id);
            if (p_Task == null)
            {
                return NotFound();
            }

            return View(p_Task);
        }

        // GET: P_Task/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: P_Task/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("P_TaskId,P_TaskName,Comentary,P_TaskState")] P_Task p_Task)
        {
            if (ModelState.IsValid)
            {
                _context.Add(p_Task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(p_Task);
        }

        // GET: P_Task/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var p_Task = await _context.P_Task.FindAsync(id);
            if (p_Task == null)
            {
                return NotFound();
            }
            return View(p_Task);
        }

        // POST: P_Task/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("P_TaskId,P_TaskName,Comentary,P_TaskState")] P_Task p_Task)
        {
            if (id != p_Task.P_TaskId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(p_Task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!P_TaskExists(p_Task.P_TaskId))
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
            return View(p_Task);
        }

        // GET: P_Task/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var p_Task = await _context.P_Task
                .FirstOrDefaultAsync(m => m.P_TaskId == id);
            if (p_Task == null)
            {
                return NotFound();
            }

            return View(p_Task);
        }

        // POST: P_Task/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var p_Task = await _context.P_Task.FindAsync(id);
            _context.P_Task.Remove(p_Task);
            await _context.SaveChangesAsync();

            ViewBag.Title = "Task deleted";
            ViewBag.Message = "Task sucessfully deleted.";
            return View("Success");
        }

        private bool P_TaskExists(int id)
        {
            return _context.P_Task.Any(e => e.P_TaskId == id);
        }
    }
}

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
using Microsoft.AspNetCore.Authorization;

namespace FinalProject.Controllers
{
    public class FunctionsController : Controller
    {
        private readonly ProjectManaContext _context;

        public FunctionsController(ProjectManaContext context)
        {
            _context = context;
        }

        // GET: Functions
        public async Task<IActionResult> Index(string fname = null, int page = 1)
        {
            var functionSearch = _context.Function
                .Where(f => fname == null || f.Name.Contains(fname));

            var pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                TotalItems = functionSearch.Count()
            };

            if (pagingInfo.CurrentPage > pagingInfo.TotalPages)
            {
                pagingInfo.CurrentPage = pagingInfo.TotalPages;
            }

            if (pagingInfo.CurrentPage < 1)
            {
                pagingInfo.CurrentPage = 1;
            }

            var functions = await functionSearch
                            .OrderBy(f => f.Name)
                            .Skip((pagingInfo.CurrentPage - 1) * pagingInfo.PageSize)
                            .Take(pagingInfo.PageSize)
                            .ToListAsync();

            return View(
                new FunctionListViewModel
                {
                    Functions = functions,
                    PagingInfo = pagingInfo,
                    StringSearched = fname
                }
            );
        }

        // GET: Functions/Details/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var function = await _context.Function
                .Include(f => f.Members)
                .FirstOrDefaultAsync(m => m.FunctionId == id);
            if (function == null)
            {
                return NotFound();
            }

            return View(function);
        }

        // GET: Functions/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Functions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FunctionId,Name")] Function function)
        {
            var functionUnique = _context.Function.Where(f => f.Name.Equals(function.Name)).Count();

            if (functionUnique != 0)
            {
                ModelState.AddModelError("Name", "Function already exists");
            }

            if (ModelState.IsValid)
            {
                _context.Add(function);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(function);
        }

        // GET: Functions/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var function = await _context.Function.FindAsync(id);
            if (function == null)
            {
                return NotFound();
            }
            return View(function);
        }

        // POST: Functions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FunctionId,Name")] Function function)
        {
            if (id != function.FunctionId)
            {
                return NotFound();
            }

            var functionUnique = _context.Function.Where(f => f.Name.Equals(function.Name) && f.FunctionId != function.FunctionId).Count();

            if (functionUnique != 0)
            {
                ModelState.AddModelError("Name", "Function already exists");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(function);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FunctionExists(function.FunctionId))
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
            return View(function);
        }

        // GET: Functions/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var function = await _context.Function
                .FirstOrDefaultAsync(m => m.FunctionId == id);
            if (function == null)
            {
                return NotFound();
            }

            return View(function);
        }

        // POST: Functions/Delete/5
        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var function = await _context.Function.FindAsync(id);
            _context.Function.Remove(function);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FunctionExists(int id)
        {
            return _context.Function.Any(e => e.FunctionId == id);
        }
    }
}

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
using Microsoft.AspNetCore.Identity;

namespace FinalProject.Controllers
{
    public class MembersController : Controller
    {
        private readonly ProjectManaContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public MembersController(
            ProjectManaContext context,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager
        )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: Members
        public async Task<IActionResult> Index(string search = null, int page = 1)
        {

            var membersSearch = _context.Member
                .Where(m => search == null || m.Name.Contains(search) || m.EmployeeNumber.Contains(search) || m.Function.Name.Contains(search));

            var pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                TotalItems = membersSearch.Count()
            };

            if (pagingInfo.CurrentPage > pagingInfo.TotalPages)
            {
                pagingInfo.CurrentPage = pagingInfo.TotalPages;
            }

            if (pagingInfo.CurrentPage < 1)
            {
                pagingInfo.CurrentPage = 1;
            }

            var members = await membersSearch
                            .Include(m => m.Function)
                            .OrderBy(m => m.Name)
                            .Skip((pagingInfo.CurrentPage - 1) * pagingInfo.PageSize)
                            .Take(pagingInfo.PageSize)
                            .ToListAsync();

            return View(
                new MemberListViewModel
                {
                    Members = members,
                    PagingInfo = pagingInfo,
                    StringSearched = search
                }
            );
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member
                .Include(m => m.Function)
                .FirstOrDefaultAsync(m => m.MemberId == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Members/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            ViewData["FunctionId"] = new SelectList(_context.Function, "FunctionId", "Name");
            ViewData["Roles"] = new List<string>(){ "Normal Member", "Project Manager" };
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateMemberViewModel member)
        {
            var memberUnique = _context.Member
                .Where(m => m.Email.Equals(member.Email)).Count();

            if (memberUnique != 0)
            {
                ModelState.AddModelError("Email", "Email already in use");
            }

            memberUnique = _context.Member
                .Where(m => m.PhoneNumber.Equals(member.PhoneNumber)).Count();

            if (memberUnique != 0)
            {
                ModelState.AddModelError("PhoneNumber", "Phone Number already in use");
            }

            memberUnique = _context.Member
                .Where(m => m.EmployeeNumber.Equals(member.EmployeeNumber)).Count();

            if (memberUnique != 0)
            {
                ModelState.AddModelError("EmployeeNumber", "Employee Number already exists");
            }

            if (!member.Password.Equals(member.ConfirmPassword))
            {
                ModelState.AddModelError("ConfirmPassword", "The password and confirmation password do not match");
            }

            if (member.Password.Length<6)
            {
                ModelState.AddModelError("Password", "The password must be at least 6 characters long");
            }

            var user = new IdentityUser { UserName = member.Email, Email = member.Email };
            var result = await _userManager.CreateAsync(user, member.Password);

            if (result.Succeeded)
            {
                if (member.Role.Equals("Normal Member"))
                {
                    await _userManager.AddToRoleAsync(user, "member");
                }
                else if (member.Role.Equals("Project Manager"))
                {
                    await _userManager.AddToRoleAsync(user, "manager");
                }

                _context.Add(new Member
                {
                    EmployeeNumber = member.EmployeeNumber,
                    Email = member.Email,
                    Name = member.Name,
                    PhoneNumber = member.PhoneNumber,
                    FunctionId = member.FunctionId
                });
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Members");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(member);
        }

        // GET: Members/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            ViewData["FunctionId"] = new SelectList(_context.Function, "FunctionId", "Name", member.FunctionId);
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MemberId,Name,Email,PhoneNumber,EmployeeNumber,FunctionId")] Member member)
        {
            if (id != member.MemberId)
            {
                return NotFound();
            }

            var memberUnique = _context.Member.Where(m => m.Email.Equals(member.Email) && m.MemberId != member.MemberId).Count();

            if (memberUnique != 0)
            {
                ModelState.AddModelError("Email", "Email already in use");
            }


            memberUnique = _context.Member
                .Where(m => m.PhoneNumber.Equals(member.PhoneNumber) && m.MemberId != member.MemberId).Count();

            if (memberUnique != 0)
            {
                ModelState.AddModelError("PhoneNumber", "Phone Number already in use");
            }

            memberUnique = _context.Member
                .Where(m => m.EmployeeNumber.Equals(member.EmployeeNumber) && m.MemberId != member.MemberId).Count();

            if (memberUnique != 0)
            {
                ModelState.AddModelError("EmployeeNumber", "Employee Number already exists");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.MemberId))
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
            ViewData["FunctionId"] = new SelectList(_context.Function, "FunctionId", "Name", member.FunctionId);
            return View(member);
        }

        
        private bool MemberExists(int id)
        {
            return _context.Member.Any(e => e.MemberId == id);
        }
    }
}

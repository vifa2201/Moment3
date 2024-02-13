using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Moment3.Data;
using Moment3.Models;

namespace Moment3.Controllers
{
    public class LoanController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoanController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Loan
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Loans.Include(l => l.Book).Include(l => l.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Loan/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanModel = await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loanModel == null)
            {
                return NotFound();
            }

            return View(loanModel);
        }

        // GET: Loan/Create
   public IActionResult Create()
{
    ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title");
    ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
    return View();
}

        // POST: Loan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BookId,UserId,LoanDate")] LoanModel loanModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loanModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title", loanModel.BookId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", loanModel.UserId);
            return View(loanModel);
        }

        // GET: Loan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanModel = await _context.Loans.FindAsync(id);
            if (loanModel == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title", loanModel.BookId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", loanModel.UserId);
            return View(loanModel);
        }

        // POST: Loan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BookId,UserId,LoanDate")] LoanModel loanModel)
        {
            if (id != loanModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loanModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoanModelExists(loanModel.Id))
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
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title", loanModel.BookId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", loanModel.UserId);
            return View(loanModel);
        }

        // GET: Loan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanModel = await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loanModel == null)
            {
                return NotFound();
            }

            return View(loanModel);
        }

        // POST: Loan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loanModel = await _context.Loans.FindAsync(id);
            if (loanModel != null)
            {
                _context.Loans.Remove(loanModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoanModelExists(int id)
        {
            return _context.Loans.Any(e => e.Id == id);
        }
    }
}

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
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _context;
        //läser ut sökvägar
        private readonly IWebHostEnvironment _hostEnvirmoment;
        private readonly string  wwwRootPath;

        public BookController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvirmoment = hostEnvironment;
            //definerar sökväg till wwwroot
            wwwRootPath = hostEnvironment.WebRootPath;
        }

// GET: Book
public async Task<IActionResult> Index(string searchString)
{
    // Hämta alla böcker med inkluderade författare och kategorier
    var books = from b in _context.Books
                select b;

    // Om en söksträng har tillhandahållits, filtrera böckerna baserat på söksträngen
    if (!string.IsNullOrEmpty(searchString))
    {
        books = books.Where(b =>
            b.Title.Contains(searchString) ||
            b.Description.Contains(searchString) ||
            b.Author.Name.Contains(searchString) ||
            b.Category.Name.Contains(searchString));
    }

    // Materialisera resultatet till en lista och skicka till vyn
    return View(await books.Include(b => b.Author).Include(b => b.Category).ToListAsync());
}

        // GET: Book/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookModel = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookModel == null)
            {
                return NotFound();
            }

            return View(bookModel);
        }

        // GET: Book/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Book/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,PublishDate,ImageFile,CategoryId,AuthorId")] BookModel bookModel)
        {
            if (ModelState.IsValid)
            {
                // kolla efter bild
                if(bookModel.ImageFile != null){
                    //genererar unik filnamn
                    string fileName = Path.GetFileNameWithoutExtension(bookModel.ImageFile.FileName);
                    string extension = Path.GetExtension(bookModel.ImageFile.FileName);

                   bookModel.ImageName = fileName = fileName.Replace(" ", String.Empty) + DateTime.Now.ToString("yymmssfff") + extension;

                   string path = Path.Combine(wwwRootPath + "/images", fileName);

                   //spara i filsystem
                   using(var fileStream = new FileStream(path, FileMode.Create)){
                            await bookModel.ImageFile.CopyToAsync(fileStream);
                   }
                }else {
                    bookModel.ImageName = "placeholder.jpg";
                }
                _context.Add(bookModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name", bookModel.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", bookModel.CategoryId);
            return View(bookModel);
        }

        // GET: Book/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookModel = await _context.Books.FindAsync(id);
            if (bookModel == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name", bookModel.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", bookModel.CategoryId);
            return View(bookModel);
        }

        // POST: Book/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,PublishDate,ImageName,CategoryId,AuthorId")] BookModel bookModel)
        {
            if (id != bookModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookModelExists(bookModel.Id))
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
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name", bookModel.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", bookModel.CategoryId);
            return View(bookModel);
        }

        // GET: Book/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookModel = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookModel == null)
            {
                return NotFound();
            }

            return View(bookModel);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookModel = await _context.Books.FindAsync(id);
            if (bookModel != null)
            {
                _context.Books.Remove(bookModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookModelExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FPTBook.Models;

namespace FPTBook.Controllers
{
    public class BookController : Controller
    {
        private readonly FptbookContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BookController(FptbookContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                   + "_"
                   + Guid.NewGuid().ToString().Substring(0, 4)
                   + Path.GetExtension(fileName);
        }

        // GET: Book
        public async Task<IActionResult> Index()
        {
            var fptbookContext = _context.Books.Include(b => b.Cat);
            return View(await fptbookContext.ToListAsync());
        }

        // GET: Book/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Cat)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Book/Create
        public IActionResult Create()
        {
            ViewData["CatId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Book/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Author,Quantity,Price,Description,Image,CatId,ImageFile")] Book Book)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = GetUniqueFileName(Book.ImageFile.FileName);
                string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Book.ImageFile.CopyToAsync(fileStream);
                }
                Book.Image = uniqueFileName;

                _context.Add(Book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Trinh function
            ViewData["CatId"] = new SelectList(_context.Categories, "Id", "Name", Book.CatId);
            return View(Book);
        }

        // GET: Book/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["CatId"] = new SelectList(_context.Categories, "Id", "Name", book.CatId);
            return View(book);
        }

        // POST: Book/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Author,Quantity,Price,Description,Image,CatId,ImageFile")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string uniqueFileName = GetUniqueFileName(book.ImageFile.FileName);
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await book.ImageFile.CopyToAsync(fileStream);
                    }
                    book.Image = uniqueFileName;

                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
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
            ViewData["CatId"] = new SelectList(_context.Categories, "Id", "Name", book.CatId);
            return View(book);
        }

        // GET: Book/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            // if (id == null || _context.Books == null)
            // {
            //     return NotFound();
            // }

            // var book = await _context.Books
            //     .Include(b => b.Cat)
            //     .FirstOrDefaultAsync(m => m.Id == id);
            // if (book == null)
            // {
            //     return NotFound();
            // }

            // return View(book);
            if (_context.Books == null)
            {
                return Problem("Entity set 'FptbookContext.Books'  is null.");
            }
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'FptbookContext.Books'  is null.");
            }
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return (_context.Books?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

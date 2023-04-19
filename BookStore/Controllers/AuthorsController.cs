using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookStore.Data;
using BookStore.Models;
using Microsoft.Extensions.Hosting;

namespace BookStore.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment Environment;

        public AuthorsController(ApplicationDbContext context, IWebHostEnvironment _environment)
        {
            _context = context;
            Environment = _environment;
        }

        // GET: Authors
        public async Task<IActionResult> Index()
        {
              return _context.Authors != null ? 
                          View(await _context.Authors.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Authors'  is null.");
        }



        // GET: Authors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Authors == null)
            {
                return NotFound();
            }

            var author = await _context.Authors
                .FirstOrDefaultAsync(m => m.AuthorId == id);
            if (author == null)
            {
                return NotFound();
            }

            var books = _context.Books.Where(book => book.AuthorId.Equals(id)).ToList();

            ViewBag.books = books;
            return View(author);
        }


        // GET: Authors/Create
        public IActionResult Create()
        {
            return View();
        }


        // POST: Authors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AuthorId,Name,Address,AuthorImage")] Author author)
        {
            if (ModelState.IsValid)
            {
                var new_author = new Author
                {
                    Name = author.Name,
                    Address = author.Address,
                };

                if (author.AuthorImage != null && author.AuthorImage.Length > 0)
                {
                    var date = DateTime.Now.ToString("yyyyMMddHHmmss");
                    var fileName = date + Path.GetFileName(author.AuthorImage.FileName);
                    var filePath = Path.Combine(Environment.WebRootPath, "images", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await author.AuthorImage.CopyToAsync(fileStream);
                    }
                    new_author.AuthorImageUrl = "/images/" + fileName;
                }

                _context.Add(new_author);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // GET: Authors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Authors == null)
            {
                return NotFound();
            }

            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }



        // POST: Authors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AuthorId,Name,Address,AuthorImageUrl")] Author author)
        {
            if (id != author.AuthorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(author);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(author.AuthorId))
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
            return View(author);
        }

        // GET: Authors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Authors == null)
            {
                return NotFound();
            }

            var author = await _context.Authors
                .FirstOrDefaultAsync(m => m.AuthorId == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }



        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Authors == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Authors'  is null.");
            }
            var author = await _context.Authors.FindAsync(id);
            if (author != null)
            {
                // Remove all books associated with the author
                var booksToRemove = _context.Books.Where(b => b.Author.AuthorId == id);
                _context.Books.RemoveRange(booksToRemove);

                // Remove the author
                _context.Authors.Remove(author);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorExists(int id)
        {
          return (_context.Authors?.Any(e => e.AuthorId == id)).GetValueOrDefault();
        }
    }
}

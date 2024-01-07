using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication9.DAL;
using WebApplication9.Models;

namespace WebApplication9.Areas.Admin.Controllers
{
    [Area("admin")]
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;

        public BlogController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Blog> blogs = await _context.Blogs.Include(x => x.Category).ToListAsync();

            return View(blogs);
        }

        [HttpGet]

        public async Task<IActionResult> Create()
        {
            ViewBag.Category = await _context.Categories.ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blog blog)
        {
            ViewBag.Category = await _context.Categories.ToListAsync();

            if (!ModelState.IsValid) return View();

            if (await _context.Blogs.AnyAsync(x => x.Title.ToLower().Trim() == blog.Title.ToLower().Trim())) 
            {
                ModelState.AddModelError("Title", $"{blog.Title} is already exists");

                return View();
            }

            if (!await _context.Categories.AnyAsync(p => p.ID == blog.CategoryID))
            {
                ModelState.AddModelError("CategoryID", $"Please select a correct category");

                return View();
            }

            blog.Title = blog.Title.Trim();
            blog.Author = blog.Author.Trim();
            blog.Date = DateTime.UtcNow;
            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? ID)
        {
            ViewBag.Category = await _context.Categories.ToListAsync();

            if (ID == null) return BadRequest();

            Blog blog = await _context.Blogs.Include(x => x.Category).FirstOrDefaultAsync(x => x.ID == ID);

            if (blog == null) return NotFound();

            return View(blog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? ID,Blog blog)
        {
            ViewBag.Category = await _context.Categories.ToListAsync();

            if (ID == null) return BadRequest();

            if (!await _context.Categories.AnyAsync(x => x.ID == blog.CategoryID))
            {
                ModelState.AddModelError("CategoryID", $"Please select a correct category");

                return View();
            }

            Blog blogs = await _context.Blogs.FirstOrDefaultAsync(x => x.ID == ID);

            if (blogs == null) return NotFound();

            blogs.Title = blog.Title.Trim();
            blogs.Author = blog.Author.Trim();
            blogs.CategoryID = blog.CategoryID;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? ID)
        {
            if (ID == null) return BadRequest();

            Blog blog = await _context.Blogs.FirstOrDefaultAsync(x => x.ID == ID);

            if (blog == null) return NotFound();

            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();

            var remainingBlogs = await _context.Blogs.ToListAsync();

            return PartialView("_BlogIndexPartial", remainingBlogs);
        }
    }
}

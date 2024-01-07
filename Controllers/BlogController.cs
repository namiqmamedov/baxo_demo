using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication9.DAL;
using WebApplication9.Models;
using WebApplication9.ViewModels.BlogViewModels;

namespace WebApplication9.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;

        public BlogController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();

            BlogVM blogVM = new BlogVM
            {
                Blog = await _context.Blogs
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.ID == id),

                Categories = await _context.Categories.Include(x=>x.Blogs).ToListAsync()
            };

            if (blogVM == null) return BadRequest();

            return View(blogVM);
        }
    }
}

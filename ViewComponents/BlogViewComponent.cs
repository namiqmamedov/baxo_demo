using Microsoft.AspNetCore.Mvc;
using WebApplication9.DAL;
using WebApplication9.Models;

namespace WebApplication9.ViewComponents
{
    public class BlogViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(IEnumerable<Blog> blogs)
        {
            return View(await Task.FromResult(blogs));
        }
    }
}

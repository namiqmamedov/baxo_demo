using WebApplication9.Models;

namespace WebApplication9.ViewModels.BlogViewModels
{
    public class BlogVM
    {
        public Blog Blog { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}

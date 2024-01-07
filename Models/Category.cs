namespace WebApplication9.Models
{
    public class Category : BaseEntity
    {
        public int ID { get; set; }
        public string Name{ get; set; }
        public ICollection<Blog> Blogs { get; set; }
    }
}

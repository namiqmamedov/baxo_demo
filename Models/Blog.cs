using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication9.Models
{
    public class Blog
    {
        public int ID { get; set; }
        
        [StringLength(255)]
        [Required]
        public string Title { get; set; }
        [StringLength(255)]
        [Required]
        public string Author { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime? Date { get; set; }
        public int? CategoryID { get; set; }
        public Category Category { get; set; }
        public string Image { get; set; }

        [NotMapped]
        [DisplayName("Image")]
        public IFormFile File { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace MyApp.Models
{
    public class Category
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string Name { get; set; }
        [Range(1,1000)]
        public int DisplayOrder { get; set; }
        public DateTime CreatedDateTime { get; set; } =DateTime.Now;
    }
}

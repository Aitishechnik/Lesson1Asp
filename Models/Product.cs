using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Lesson1.Models
{
    public class Product
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }
        public float Calories { get; set; }
        public DateTime Shelflife { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
    }
}

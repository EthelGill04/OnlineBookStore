using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OnlineBookStore.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; } = "";
        [Required]
        public string Description { get; set; } = "";
        [Required, MaxLength(100)]
        public string Author { get; set; } = "";
        [Required, Precision(16,0)]
        public int Price { get; set; }
        [Required]
        public DateTime DatePublished { get; set; }
        [Required, MaxLength(100)]
        public string Language { get; set; } = "";
        [Required, MaxLength(100)]
        public string Genre { get; set; } = "";
        [Required]
        public string Image { get; set; } = "";
    }
}

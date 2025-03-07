using System.ComponentModel.DataAnnotations;

namespace OnlineBookStore.Models
{
    public class AddProductDto
    {
        [Required, MaxLength(100)]
        public string Title { get; set; } = "";

        [Required]
        public string Description { get; set; } = "";

        [Required, MaxLength(100)]
        public string Author { get; set; } = "";

        [Required]
        public int Price { get; set; }

        [Required, MaxLength(100)]
        public string Language { get; set; } = "";

        [Required, MaxLength(100)]
        public string Genre { get; set; } = "";

        public IFormFile? ImageFile { get; set; }
    }
}

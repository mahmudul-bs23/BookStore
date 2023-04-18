using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }
        [Required]
        public string? Title { get; set; }
        public string? Language { get; set; }
        [NotMapped]
        public IFormFile ?BookImage { get; set; }
        public string ?BookImageUrl { get; set; }
        public int AuthorId { get; set; }
        
        public virtual Author? Author { get; set; }

    }
}

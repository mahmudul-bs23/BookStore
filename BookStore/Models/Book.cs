using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Image { get; set; }
        public string Language { get; set; }
        public Author Author { get; set; }

    }
}

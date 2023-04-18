using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        [NotMapped]
        public IFormFile ?AuthorImage { get; set; }
        public string ?AuthorImageUrl { get; set; }

        public virtual ICollection<Book> ?Books { get; set; }

    }
}

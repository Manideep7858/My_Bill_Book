using System.ComponentModel.DataAnnotations;

namespace My_Bill_Book.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? Phone { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}

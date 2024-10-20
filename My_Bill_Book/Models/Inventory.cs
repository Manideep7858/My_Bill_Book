using System.ComponentModel.DataAnnotations;

namespace My_Bill_Book.Models
{
    public class Inventory
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}

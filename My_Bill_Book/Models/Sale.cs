using System.ComponentModel.DataAnnotations;

namespace My_Bill_Book.Models
{
    public class Sale
    {
        public int Id { get; set; }

        public DateTime SaleDate { get; set; }

        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal TotalAmount { get; set; }

        public List<SaleItem>? SaleItems { get; set; }
    }

    public class SaleItem
    {
        public int Id { get; set; }

        public int SaleId { get; set; }
        public Sale? Sale { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal UnitPrice { get; set; }
    }
}

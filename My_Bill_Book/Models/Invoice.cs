using System.ComponentModel.DataAnnotations;

namespace My_Bill_Book.Models
{
    public class Invoice
    {
        public int Id { get; set; }

        public int SaleId { get; set; }
        public Sale? Sale { get; set; }

        [Required]
        public string? InvoiceNumber { get; set; }

        public DateTime InvoiceDate { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal TotalAmount { get; set; }

        public bool IsPaid { get; set; }
    }
}

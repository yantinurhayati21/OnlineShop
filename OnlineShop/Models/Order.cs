using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        public Product Product { get; set; }

        public int qty { get; set; }

        public string Status { get; set; } = "Pesanan Belum Diterima";


    }

    public class OrderForm
    {

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        public int ProductId { get; set; }

        public int qty { get; set; }

        public string Status { get; set; } = "Pesanan Belum Diterima";

    }

}
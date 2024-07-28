using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Username harus diinput.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Username minimal 3 karakter")]
        public string Username { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password minimal 8 karakter")]
        public string Password { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public string Alamat { get; set; }

        [Required]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Format email tidak valid")]
        public string Email { get; set; }

        [Required]
        // [RegularExpression(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$")]
        public string Telepon { get; set; }

        [Required]
        public DateTime TanggalLahir { get; set; }

        [NotMapped]
        public string FormatTanggalLahir { get; set; }

        public string Foto { get; set; }
    }

    public class UserForm
    {
        [Required(ErrorMessage = "Username harus diinput.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Username minimal 3 karakter")]
        public string Username { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password minimal 8 karakter")]
        public string Password { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public string Alamat { get; set; }

        [Required]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Format email tidak valid")]
        public string Email { get; set; }

        [Required]
        public string Telepon { get; set; }

        [Required]
        public DateTime TanggalLahir { get; set; }
    }
}

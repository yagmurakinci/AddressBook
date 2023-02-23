using System.ComponentModel.DataAnnotations;

namespace AddressBookPL.Models
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Adınız en az 2 en çok 50 karakter olmalıdır!")]
        public string Name { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Soyadınız en az 2 en çok 50 karakter olmalıdır!")]
        public string Surname { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string UserName { get; set; }
        //[StringLength(8, MinimumLength = 8, ErrorMessage = "Parola 8 karakter olmalıdır")]
        //[RegularExpression(@"^[a-z][a-z0-9_-]*$", ErrorMessage = @"Parola küçük harf ile başlamalıdır.Sonrasında küçük harf,rakam, tire ya da alt tire kullanılabilir. ")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor!")]
        public string ConfirmPassword { get; set; }

        public string PhoneNumber { get; set; }
    }
}

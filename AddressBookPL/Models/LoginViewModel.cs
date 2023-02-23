using System.ComponentModel.DataAnnotations;

namespace AddressBookPL.Models
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Kullanıcı adı min ve mak karakter aralığına göre giriş yapmalısınız!")]
        public string Username { get; set; }
        [Required]
        //[StringLength(8, MinimumLength = 8, ErrorMessage = "Parolanız 8 karakter olmak zorundadır!")]
        public string Password { get; set; }
        public bool  RememberMe { get; set; }
    }
}

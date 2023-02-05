using System.ComponentModel.DataAnnotations;

namespace QL_BanGiay.Models
{
    public class LoginModel
    {
        [EmailAddress]
        [Required(ErrorMessage = "Vui lòng nhập email của bạn")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        public string? Password { get; set; }
        public bool Remember { set; get; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace QL_BanGiay.Models
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Nhập mật khẩu cũ của bạn")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*[#$^+=!*()@%&]).{6,}$", ErrorMessage = "Nhập ít nhất 6 kí tự, 1 kí tự viết hoa, 1 kí tự viết thường, 1 kí tự đặc biệt và 1 số")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Nhập mật mới của bạn")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*[#$^+=!*()@%&]).{6,}$", ErrorMessage = "Nhập ít nhất 6 kí tự, 1 kí tự viết hoa, 1 kí tự viết thường, 1 kí tự đặc biệt và 1 số")]
        public string? New_Password { get; set;}
        [Required(ErrorMessage = "Nhập lại mật khẩu")]
        [Compare("New_Password", ErrorMessage = "Mật khẩu nhập lại không chính xác")]
        public string? Confirm_Password { get; set;}
    }
}

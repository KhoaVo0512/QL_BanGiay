using QL_BanGiay.Data;
using System.ComponentModel.DataAnnotations;

namespace QL_BanGiay.Models
{
    public class RegisterModel
    { 
        [Required(ErrorMessage = "Nhập mật khẩu")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*[#$^+=!*()@%&]).{6,}$", ErrorMessage = "Nhập ít nhất 6 kí tự, 1 kí tự viết hoa, 1 kí tự viết thường, 1 kí tự đặc biệt và 1 số")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Nhập lại mật khẩu")]
        [Compare("Password")]
        public string? PasswordConfirm { get; set; }
        [Required(ErrorMessage = "Nhập họ của bạn")]
        public string? HoNguoiDung { get; set; }
        [Required(ErrorMessage = "Nhập tên của bạn")]
        public string? TenNguoiDung { get; set; }
        [Required(ErrorMessage = "Nhập địa chỉ Email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Nhập số điện thoại")]
        [Phone(ErrorMessage = "Nhập đúng định dạng số điện thoại")]
        public string? Sdt { get; set; }

        public int? GioiTinh { get; set; }
        [Required(ErrorMessage = "Chọn tỉnh hoăc thành phố")]
        public string? MaTinh { get; set; }
        [Required(ErrorMessage = "Chọn huyện hoặc quận")]
        public string? MaHuyen { get; set; }
        [Required(ErrorMessage = "Chọn xã hoặc phường")]
        public string? MaXa { get; set; }
        [Required(ErrorMessage = "Nhập địa chỉ")]
        public string? DiaChi { get; set; }

    }
}

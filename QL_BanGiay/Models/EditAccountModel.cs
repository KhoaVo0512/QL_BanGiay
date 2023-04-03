using System.ComponentModel.DataAnnotations;

namespace QL_BanGiay.Models
{
    public class EditAccountModel
    {
        
        public string? MaNguoiDung { get; set; }
        public int? MaDiaChi { get; set; }
        [Required(ErrorMessage = "Nhập họ của bạn")]
        public string? HoNguoiDung { get; set; }
        [Required(ErrorMessage = "Nhập tên của bạn")]
        public string? TenNguoiDung { get; set; }
        [Required(ErrorMessage = "Nhập địa chỉ Email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Nhập số điện thoại")]
        [Phone(ErrorMessage = "Nhập đúng định dạng số điện thoại")]
        [RegularExpression(@"((^(\+84|84|0|0084){1})(3|5|7|8|9))+([0-9]{8})$", ErrorMessage = "Nhập sai số điện thoại")]
        public string? Sdt { get; set; }

        [Required(ErrorMessage = "Chọn tỉnh hoặc thành phố")]
        public string? MaTinh { get; set; }
        [Required(ErrorMessage = "Chọn huyện hoặc quận")]
        public string? MaHuyen { get; set; }
        [Required(ErrorMessage = "Chọn xã hoặc phường")]
        public string? MaXa { get; set; }
        [Required(ErrorMessage = "Nhập địa chỉ")]
        public string? DiaChi { get; set; }
    }
}

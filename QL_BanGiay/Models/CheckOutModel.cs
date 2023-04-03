using QL_BanGiay.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QL_BanGiay.Models
{
    public class CheckOutModel
    {
        [Required(ErrorMessage = "Nhập họ của bạn")]
        public string? Ho { get; set; }
        [Required(ErrorMessage = "Nhập tên của bạn")]
        public string? Ten { get; set; }
        [Required(ErrorMessage = "Nhập địa chỉ Email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Nhập số điện thoại")]
        [Phone(ErrorMessage = "Nhập đúng định dạng số điện thoại")]
        [RegularExpression(@"((^(\+84|84|0|0084){1})(3|5|7|8|9))+([0-9]{8})$", ErrorMessage = "Nhập sai số điện thoại")]
        public string? Sdt { get; set; }
        [Required(ErrorMessage = "Chọn tỉnh hoăc thành phố")]
        public string? MaTinh { get; set; }
        [Required(ErrorMessage = "Chọn huyện hoặc quận")]
        public string? MaHuyen { get; set; }
        [Required(ErrorMessage = "Chọn xã hoặc phường")]
        public string? MaXa { get; set; }
        [Required(ErrorMessage = "Nhập địa chỉ")]
        public string? DiaChi { get; set; }
        public string? GhiChu { get; set ; }
        [NotMapped]
        public List<string>? DiaChis { get; set; }
        [NotMapped]
        public List<int>? IdDiaChi { get; set; }
    }
}

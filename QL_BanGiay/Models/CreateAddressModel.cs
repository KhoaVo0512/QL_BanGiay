using System.ComponentModel.DataAnnotations;

namespace QL_BanGiay.Models
{
    public class CreateAddressModel
    {
        [Required(ErrorMessage = "Chọn tỉnh hoặc thành phố")]
        public string? MaTinh { get; set; }
        [Required(ErrorMessage = "Chọn huyện hoặc quận")]
        public string? MaHuyen { get; set; }
        [Required(ErrorMessage = "Chọn xã hoặc phường")]
        public string? MaXa { get; set; }
        [Required(ErrorMessage = "Nhập địa chỉ")]
        public string? DiaChi { get; set; }
        public int? MaDiaChi { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace QL_BanGiay.Areas.Admin.Models
{
    public class ShoeContext
    {
        [Required(ErrorMessage = "Vui lòng nhập mã giày")]
        public string MaGiay { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn nhà sản xuất giày")]
        public int? MaNhaSanXuat { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn nhãn hiệu")]
        public int? MaNhanHieu { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn bộ sưu tập giày")]
        public int? MaDongSanPham { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên giày")]
        public string? TenGiay { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập chất liệu giày")]
        public string? ChatLieu { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập màu sắc giày")]
        public string? MauSac { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn hình ảnh cho sản phẩm")]
        public IFormFile? MaAnhNen { get; set; }
        public string? GiaBan { get;set; }
        public string? anhNenUrl { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn hình ảnh chi tiết cho sản phẩm")]
        public IFormFileCollection? AnhDetail { get; set; }
        public List<ShoeImageContext>? Images { get; set; }
        
    }
}

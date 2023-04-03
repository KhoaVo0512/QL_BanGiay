using QL_BanGiay.Data;
using System.ComponentModel.DataAnnotations;

namespace QL_BanGiay.Areas.Admin.Models
{
    public class EditShoeModel
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
        public IFormFile? MaAnhNen { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập giá bán")]
        [Range(1, 10000000, ErrorMessage = "Giá mua phải lớn hơn 0")]
        public string? GiaBan { get; set; }
        public string? anhNenUrl { get; set; }
        public IFormFileCollection? AnhDetail { get; set; }
        public List<ShoeImageContext>? Images { get; set; }
        public virtual List<AnhGiay> AnhGiays { get; set; } = new List<AnhGiay>();
        public string? AnhDD { get;set; }
    }
}

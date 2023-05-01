
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QL_BanGiay.Data;

public partial class DongSanPham
{
    
    public int MaDongSanPham { get; set; }
    [Required(ErrorMessage = "Vui lòng chọn nhãn hiệu")]
    public int? MaNhanHieu { get; set; }
    [Required(ErrorMessage = "Vui lòng nhập tên dòng sản phẩm")]
    public string? TenDongSanPham { get; set; }

    public virtual ICollection<Giay> Giays { get; } = new List<Giay>();

    public virtual NhanHieu? MaNhanHieuNavigation { get; set; }
}

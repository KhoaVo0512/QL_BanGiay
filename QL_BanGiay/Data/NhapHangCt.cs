using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QL_BanGiay.Data;

public partial class NhapHangCt
{
    public int MaNhapHangCt { get; set; }
    [Required(ErrorMessage = "Vui lòng chọn sản phẩm")]
    public string? MaGiay { get; set; }

    public int? MaNhapHang { get; set; }
    [Required(ErrorMessage = "Vui lòng chọn size giày")]
    public int? MaSize { get; set; }
    [Required(ErrorMessage = "Vui lòng nhập số lượng")]
    [Range(1, 1000, ErrorMessage = "Số lượng phải từ 1 đến 1000")]
    public int? SoLuong { get; set; }
    [Required(ErrorMessage = "Vui lòng nhập giá mua")]
    [Range(1, 10000000, ErrorMessage = "Giá mua phải lớn hơn 0")]
    public int? GiaMua { get; set; }

    public virtual Giay? MaGiayNavigation { get; set; }

    public virtual NhapHang? MaNhapHangNavigation { get; set; }

    public virtual SizeGiay? MaSizeNavigation { get; set; }
    [NotMapped]
    public bool IsDeleted { get; set; } = false;
    [MaxLength(75)]
    [NotMapped]
    [Required(ErrorMessage = "Vui lòng chọn sản phẩm")]
    public string? Description { get; set; } = "";
    [NotMapped]
    public decimal Total { get; set; }
}

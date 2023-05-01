using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QL_BanGiay.Data;

public partial class NhapHang
{
    public int MaNhapHang { get; set; }
    [Required(ErrorMessage = "Vui lòng chọn nhà cung cấp")]
    public int? MaDonViNhap { get; set; }
    [Required(ErrorMessage = "Vui lòng nhập số hóa đơn")]
    public int? SoHoaDon { get; set; }
    [DataType(DataType.Date)]
    public DateTime? NgayNhap { get; set; } = DateTime.Now.Date;
    public int? TongTien { get; set; }
    public virtual DonViNhapHang? MaDonViNhapNavigation { get; set; }

    public virtual List<NhapHangCt>? NhapHangCts { get; } = new List<NhapHangCt>();
}

using System;
using System.Collections.Generic;

namespace QL_BanGiay.Data;

public partial class DonViNhapHang
{
    public int MaDonViNhap { get; set; }

    public string? TenDonViNhap { get; set; }

    public string? Email { get; set; }

    public string? DiaChi { get; set; }

    public string? SoDienThai { get; set; }

    public virtual ICollection<NhapHang> NhapHangs { get; } = new List<NhapHang>();
}

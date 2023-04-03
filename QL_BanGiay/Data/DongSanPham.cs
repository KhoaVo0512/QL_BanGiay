using System;
using System.Collections.Generic;

namespace QL_BanGiay.Data;

public partial class DongSanPham
{
    public int MaDongSanPham { get; set; }

    public int MaNhanHieu { get; set; }

    public string TenDongSanPham { get; set; } = null!;

    public virtual ICollection<Giay> Giays { get; } = new List<Giay>();

    public virtual NhanHieu MaNhanHieuNavigation { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace QL_BanGiay.Data;

public partial class NhanHieu
{
    public int MaNhanHieu { get; set; }

    public string? TenNhanHieu { get; set; }

    public virtual ICollection<DongSanPham> DongSanPhams { get; } = new List<DongSanPham>();
}

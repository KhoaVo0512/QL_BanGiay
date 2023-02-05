using System;
using System.Collections.Generic;

namespace QL_BanGiay.Data;

public partial class NoiSanXuat
{
    public int MaNhaSanXuat { get; set; }

    public string? TenNhaSanXuat { get; set; }

    public virtual ICollection<Giay> Giays { get; } = new List<Giay>();
}

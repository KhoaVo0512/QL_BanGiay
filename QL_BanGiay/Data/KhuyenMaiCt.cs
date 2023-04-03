using System;
using System.Collections.Generic;

namespace QL_BanGiay.Data;

public partial class KhuyenMaiCt
{
    public int Id { get; set; }

    public string? MaGiay { get; set; }

    public string? MaKhuyenMai { get; set; }

    public double? Tile { get; set; }

    public int? GiaHienTai { get; set; }

    public virtual Giay? MaGiayNavigation { get; set; }

    public virtual KhuyenMai? MaKhuyenMaiNavigation { get; set; }
}

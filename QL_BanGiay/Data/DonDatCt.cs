using System;
using System.Collections.Generic;

namespace QL_BanGiay.Data;

public partial class DonDatCt
{
    public string? MaGiay { get; set; }

    public int? MaDonDat { get; set; }

    public int? SoLuong { get; set; }

    public double? DonGia { get; set; }

    public int Id { get; set; }

    public virtual DonDat? MaDonDatNavigation { get; set; }

    public virtual Giay? MaGiayNavigation { get; set; }
}

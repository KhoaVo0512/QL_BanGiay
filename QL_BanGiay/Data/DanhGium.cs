using System;
using System.Collections.Generic;

namespace QL_BanGiay.Data;

public partial class DanhGium
{
    public int MaDanhGia { get; set; }

    public string? MaGiay { get; set; }

    public int? Sao { get; set; }

    public string? HoTen { get; set; }

    public string? NoiDung { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual Giay? MaGiayNavigation { get; set; }
}

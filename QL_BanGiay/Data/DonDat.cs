using System;
using System.Collections.Generic;

namespace QL_BanGiay.Data;

public partial class DonDat
{
    public int MaDonDat { get; set; }

    public string? MaNguoiDung { get; set; }

    public DateTime? NgayDat { get; set; }

    public bool? DaThanhToan { get; set; }

    public virtual ICollection<DonDatCt> DonDatCts { get; } = new List<DonDatCt>();

    public virtual NguoiDung? MaNguoiDungNavigation { get; set; }
}

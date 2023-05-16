using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace QL_BanGiay.Data;

public partial class HoaDon
{
    public string MaHd { get; set; } = null!;

    public string? MaNguoiDung { get; set; }

    public double? TongTien { get; set; }

    public string? IdVnpay { get; set; }

    public DateTime? NgayLapDh { get; set; }

    public DateTime? NgayGiaoHd { get; set; }

    public string? GhiChu { get; set; }

    public string? DiaChiNhan { get; set; }

    public virtual List<HoaDonCt> HoaDonCts { get; set; } = new List<HoaDonCt>();

    public virtual NguoiDung? MaNguoiDungNavigation { get; set; }
    [NotMapped]
    public string? TenKH { get; set; }
    [NotMapped]
    public string? Sdt { get; set; }
    [NotMapped]
    public string? Email { get; set; }
}

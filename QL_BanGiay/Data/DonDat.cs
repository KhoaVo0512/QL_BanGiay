using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace QL_BanGiay.Data;

public partial class DonDat
{
    public string MaDonDat { get; set; } = null!;

    public string? MaNguoiDung { get; set; }

    public DateTime? NgayDat { get; set; }

    public string? GhiChu { get; set; }

    public string? MaVnpay { get; set; }

    public int? TrangThai { get; set; }

    public string? DiaChiNhan { get; set; }

    public virtual List<DonDatCt> DonDatCts { get; set; } = new List<DonDatCt>();

    public virtual NguoiDung? MaNguoiDungNavigation { get; set; }
    [NotMapped]
    public string? TenKH { get; set; }
    [NotMapped]
    public string? Sdt { get; set; }
    [NotMapped]
    public string? Email { get; set; }
    [NotMapped]
    public int? Total { get; set; }
}

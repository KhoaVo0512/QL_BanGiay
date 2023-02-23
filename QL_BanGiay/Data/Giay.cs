using System;
using System.Collections.Generic;

namespace QL_BanGiay.Data;

public partial class Giay
{
    public string MaGiay { get; set; } = null!;

    public int? MaNhaSanXuat { get; set; }

    public int? MaDongSanPham { get; set; }

    public string? TenGiay { get; set; }

    public string? ChatLieu { get; set; }

    public string? MauSac { get; set; }
    public string? AnhDD { get; set; }

    public int? GiaBan { get; set; }

    public DateTime? NgayCn { get; set; }

    public bool? TrangThai { get; set; }

    public virtual ICollection<AnhGiay> AnhGiays { get; set; } = new List<AnhGiay>();
    public virtual List<NoiDung> NoiDungs { get; } = new List<NoiDung>();

    public virtual ICollection<DonDatCt> DonDatCts { get; } = new List<DonDatCt>();

    public virtual List<KhoGiay> KhoGiays { get; } = new List<KhoGiay>();

    public virtual ICollection<KhuyenMaiCt> KhuyenMaiCts { get; } = new List<KhuyenMaiCt>();

    public virtual DongSanPham? MaDongSanPhamNavigation { get; set; }

    public virtual NoiSanXuat? MaNhaSanXuatNavigation { get; set; }

    public virtual ICollection<NhapHangCt> NhapHangCts { get; } = new List<NhapHangCt>();
}

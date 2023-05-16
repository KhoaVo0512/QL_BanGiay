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

    public string? AnhDd { get; set; }

    public int? GiaBan { get; set; }

    public DateTime? NgayCn { get; set; }

    public bool? TrangThai { get; set; }

    public string? NoiDung { get; set; }

    public string? TomTat { get; set; }

    public bool? NoiBat { get; set; }

    public virtual List<AnhGiay> AnhGiays { get; set; } = new List<AnhGiay>();

    public virtual ICollection<DanhGium> DanhGia { get; } = new List<DanhGium>();

    public virtual ICollection<DonDatCt> DonDatCts { get; } = new List<DonDatCt>();

    public virtual ICollection<HoaDonCt> HoaDonCts { get; } = new List<HoaDonCt>();

    public virtual List<KhoGiay> KhoGiays { get; } = new List<KhoGiay>();

    public virtual ICollection<KhuyenMaiCt> KhuyenMaiCts { get; } = new List<KhuyenMaiCt>();

    public virtual DongSanPham? MaDongSanPhamNavigation { get; set; }

    public virtual NoiSanXuat? MaNhaSanXuatNavigation { get; set; }

    public virtual List<NhapHangCt> NhapHangCts { get;set; } = new List<NhapHangCt>();
}

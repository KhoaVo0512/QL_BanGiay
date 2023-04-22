using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace QL_BanGiay.Data;

public partial class NguoiDung
{
    public string MaNguoiDung { get; set; } = null!;

    public string? Email { get; set; }

    public string? HoNguoiDung { get; set; }

    public string? TenNguoiDung { get; set; }

    public string? Sdt { get; set; }

    public DateTime? CreateAt { get; set; }

    public virtual List<DiaChi> DiaChis { get; } = new List<DiaChi>();

    public virtual List<DonDat> DonDats { get; } = new List<DonDat>();

    public virtual ICollection<HoaDon> HoaDons { get; } = new List<HoaDon>();

    public virtual List<TaiKhoan> TaiKhoans { get; } = new List<TaiKhoan>();
    [NotMapped]
    public string? Address { get; set; }
    [NotMapped]
    public List<string>? DiasChiUser { get; set; }
    [NotMapped]
    public List<int>? IdDiaChi { get; set; }
}

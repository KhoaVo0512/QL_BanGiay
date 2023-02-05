using System;
using System.Collections.Generic;

namespace QL_BanGiay.Data;

public partial class NguoiDung
{
    public string MaNguoiDung { get; set; } = null!;

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? HoNguoiDung { get; set; }

    public string? TenNguoiDung { get; set; }

    public string? Sdt { get; set; }

    public int? GioiTinh { get; set; }

    public string? Avatar { get; set; }

    public DateTime? CreateAt { get; set; }

    public virtual ICollection<DiaChi> DiaChis { get; } = new List<DiaChi>();

    public virtual ICollection<DonDat> DonDats { get; } = new List<DonDat>();

    public virtual ICollection<QuyenCt> QuyenCts { get; } = new List<QuyenCt>();
}

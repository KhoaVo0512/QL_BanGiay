using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace QL_BanGiay.Data;

public partial class TaiKhoan
{
    public string Username { get; set; } = null!;

    public string? MaNguoiDung { get; set; }

    public string Password { get; set; } = null!;

    public string AnhTk { get; set; } = null!;

    public virtual NguoiDung? MaNguoiDungNavigation { get; set; }

    public virtual ICollection<QuyenCt> QuyenCts { get; } = new List<QuyenCt>();
    [NotMapped]
    public string? HoVaTen { get; set; }
    [NotMapped]
    public string? DiaChi { get; set; }
}

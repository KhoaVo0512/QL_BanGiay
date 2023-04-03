using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QL_BanGiay.Data;

public partial class HoaDonCt
{
    public int MaHoaDonCt { get; set; }

    public string? MaGiay { get; set; }

    public int? MaSize { get; set; }

    public int? SoLuong { get; set; }

    public double? DonGia { get; set; }

    public string? MaHd { get; set; }

    public virtual Giay? MaGiayNavigation { get; set; }

    public virtual HoaDon? MaHdNavigation { get; set; }

    public virtual SizeGiay? MaSizeNavigation { get; set; }
    [MaxLength(75)]
    [NotMapped]
    public string? Description { get; set; } = "";
    [NotMapped]
    public decimal Total { get; set; }
    [NotMapped]
    public bool IsDeleted { get; set; } = false;
}

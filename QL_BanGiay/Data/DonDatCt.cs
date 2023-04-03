using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QL_BanGiay.Data;

public partial class DonDatCt
{
    public int Id { get; set; }

    public string? MaGiay { get; set; }

    public int? MaSize { get; set; }

    public string? MaDonDat { get; set; }

    public int? SoLuong { get; set; }

    public double? DonGia { get; set; }

    public virtual DonDat? MaDonDatNavigation { get; set; }

    public virtual Giay? MaGiayNavigation { get; set; }

    public virtual SizeGiay? MaSizeNavigation { get; set; }
    [MaxLength(75)]
    [NotMapped]
    public string Description { get; set; } = "";
    [NotMapped]
    public decimal Total { get; set; }
    [NotMapped]
    public bool IsDeleted { get; set; } = false;
}

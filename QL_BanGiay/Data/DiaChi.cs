﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace QL_BanGiay.Data;

public partial class DiaChi
{
    public int MaDiaChi { get; set; }

    public string? MaNguoiDung { get; set; }

    public string? MaXa { get; set; }

    public string? TenDiaChi { get; set; }

    public virtual NguoiDung? MaNguoiDungNavigation { get; set; }

    public virtual Xa? MaXaNavigation { get; set; }
    [NotMapped]
    public string? Address { get; set; }
}

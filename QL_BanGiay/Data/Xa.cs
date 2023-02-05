using System;
using System.Collections.Generic;

namespace QL_BanGiay.Data;

public partial class Xa
{
    public string MaXa { get; set; } = null!;

    public string? TenXa { get; set; }

    public string? MaHuyen { get; set; }

    public virtual ICollection<DiaChi> DiaChis { get; } = new List<DiaChi>();

    public virtual Huyen? MaHuyenNavigation { get; set; }
}

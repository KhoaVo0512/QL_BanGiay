using System;
using System.Collections.Generic;

namespace QL_BanGiay.Data;

public partial class Huyen
{
    public string MaHuyen { get; set; } = null!;

    public string? TenHuyen { get; set; }

    public string? MaTinh { get; set; }

    public virtual Tinh? MaTinhNavigation { get; set; }

    public virtual ICollection<Xa> Xas { get; } = new List<Xa>();
}

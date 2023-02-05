using System;
using System.Collections.Generic;

namespace QL_BanGiay.Data;

public partial class AnhGiay
{
    public int MaAnh { get; set; }

    public string? MaGiay { get; set; }

    public string? TenAnh { get; set; }

    public string? Url { get; set; }

    public virtual Giay? MaGiayNavigation { get; set; }
}

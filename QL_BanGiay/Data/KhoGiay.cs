using System;
using System.Collections.Generic;

namespace QL_BanGiay.Data;

public partial class KhoGiay
{
    public string MaGiay { get; set; } = null!;

    public int MaSize { get; set; }

    public int? SoLuong { get; set; }

    public virtual Giay MaGiayNavigation { get; set; } = null!;

    public virtual SizeGiay MaSizeNavigation { get; set; } = null!;
}

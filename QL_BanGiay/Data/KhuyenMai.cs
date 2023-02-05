using System;
using System.Collections.Generic;

namespace QL_BanGiay.Data;

public partial class KhuyenMai
{
    public string MaKhuyenMai { get; set; } = null!;

    public DateTime? NgayBd { get; set; }

    public DateTime? NgayKt { get; set; }

    public string? Lydo { get; set; }

    public virtual ICollection<KhuyenMaiCt> KhuyenMaiCts { get; } = new List<KhuyenMaiCt>();
}

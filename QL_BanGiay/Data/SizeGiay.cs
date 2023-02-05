using System;
using System.Collections.Generic;

namespace QL_BanGiay.Data;

public partial class SizeGiay
{
    public int MaSize { get; set; }

    public string? TenSize { get; set; }

    public virtual ICollection<KhoGiay> KhoGiays { get; } = new List<KhoGiay>();

    public virtual ICollection<NhapHangCt> NhapHangCts { get; } = new List<NhapHangCt>();
}

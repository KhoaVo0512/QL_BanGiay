using System;
using System.Collections.Generic;

namespace QL_BanGiay.Data;

public partial class Quyen
{
    public int MaQuyen { get; set; }

    public string? TenQuyen { get; set; }

    public virtual ICollection<QuyenCt> QuyenCts { get; } = new List<QuyenCt>();
}

using System;
using System.Collections.Generic;

namespace QL_BanGiay.Data;

public partial class Tinh
{
    public string MaTinh { get; set; } = null!;

    public string TenTinh { get; set; } = null!;

    public virtual ICollection<Huyen> Huyens { get; } = new List<Huyen>();
}

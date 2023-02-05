using System;
using System.Collections.Generic;

namespace QL_BanGiay.Data;

public partial class QuyenCt
{
    public int Id { get; set; }

    public string? MaNguoiDung { get; set; }

    public int MaQuyen { get; set; }

    public virtual NguoiDung? MaNguoiDungNavigation { get; set; }

    public virtual Quyen MaQuyenNavigation { get; set; } = null!;
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace QL_BanGiay.Data;

public partial class QuyenCt
{
    public int Id { get; set; }

    public int MaQuyen { get; set; }

    public string? UserName { get; set; }

    public virtual Quyen MaQuyenNavigation { get; set; } = null!;

    public virtual TaiKhoan? UserNameNavigation { get; set; }
}

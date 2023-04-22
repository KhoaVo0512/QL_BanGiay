using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QL_BanGiay.Data;

public partial class DonViNhapHang
{
    public int MaDonViNhap { get; set; }
    [Required(ErrorMessage = "Vui lòng nhập tên đơn vị nhập")]
    public string? TenDonViNhap { get; set; }
    [Required(ErrorMessage = "Vui lòng nhập địa chỉ Email")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string? Email { get; set; }
    [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
    public string? DiaChi { get; set; }
    [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
    [RegularExpression(@"((^(\+84|84|0|0084){1})(3|5|7|8|9))+([0-9]{8})$", ErrorMessage = "Nhập sai số điện thoại")]
    public string? SoDienThai { get; set; }

    public virtual ICollection<NhapHang> NhapHangs { get; } = new List<NhapHang>();
}

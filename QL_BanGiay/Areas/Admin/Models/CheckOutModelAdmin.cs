using QL_BanGiay.Data;

namespace QL_BanGiay.Areas.Admin.Models
{
    public class CheckOutModelAdmin
    {
        public HoaDon HoaDon { get; set; }
        public NguoiDung NguoiDung { get; set; }
        public DiaChi  DiaChi { get; set; }
        public Xa Xa { get; set; }
        public Huyen Huyen { get; set; }
        public Tinh Tinh { get; set; }
    }
}

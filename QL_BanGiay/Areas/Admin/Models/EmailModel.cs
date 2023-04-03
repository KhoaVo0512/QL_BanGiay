using QL_BanGiay.Data;
using System.ComponentModel.DataAnnotations;

namespace QL_BanGiay.Areas.Admin.Models
{
    public class EmailModel
    {
        public string? HoTen { get; set; }
        public string? Email { get; set; }
        public string? Sdt { get; set; }
        public string? DiaChi { get; set; }
        public string? MaHoaDon { get; set; }
        [DataType(DataType.Date)]
        public DateTime NgayDat { get; set; }
        public List<DonDatCt>? DonDatCts { get; set; }
    }
}

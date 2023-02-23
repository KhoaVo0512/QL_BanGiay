namespace QL_BanGiay.Data
{
    public class NoiDung
    {
        public int MaNoiDung { get; set; }

        public string? MaGiay { get; set; }

        public string? ThongTin { get; set; }

        public virtual Giay? MaGiayNavigation { get; set; }
    }
}

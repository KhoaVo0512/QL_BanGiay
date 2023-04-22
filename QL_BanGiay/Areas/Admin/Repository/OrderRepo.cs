using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QL_BanGiay.Areas.Admin.Interface;
using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Data;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Security.Cryptography;

namespace QL_BanGiay.Areas.Admin.Repository
{
    public class OrderRepo : IOrder
    {
        private readonly QlyBanGiayContext _context;
        public OrderRepo(QlyBanGiayContext context)
        {
            _context = context;
        }

        private List<DonDat> DoSort(List<DonDat> items, string SortProperty, SortOrder sortOrder)
        {

            if (SortProperty.ToLower() == "idorder")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.MaDonDat).ToList();
                else
                    items = items.OrderByDescending(n => n.MaDonDat).ToList();
            }
            else if (SortProperty.ToLower() == "date")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderByDescending(n => n.NgayDat).ToList();
                else
                    items = items.OrderBy(n => n.NgayDat).ToList();
            }
            else if (SortProperty.ToLower() == "name")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.MaNguoiDungNavigation.TenNguoiDung).ToList();
                else
                    items = items.OrderByDescending(n => n.MaNguoiDungNavigation.TenNguoiDung).ToList();
            }
            else if (SortProperty.ToLower() == "phone")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.MaNguoiDungNavigation.Sdt).ToList();
                else
                    items = items.OrderByDescending(n => n.MaNguoiDungNavigation.Sdt).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderByDescending(d => d.NgayDat).ToList();
                else
                    items = items.OrderBy(d => d.NgayDat).ToList();
            }

            return items;
        }
        public PaginatedList<DonDat> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 10)
        {
            List<DonDat> items;
            if (SearchText != "" && SearchText != null)
            {
                items = _context.DonDats.Where(ut => (ut.GhiChu.ToLower().Contains(SearchText.ToLower()) || ut.MaNguoiDungNavigation.TenNguoiDung.ToLower().Contains(SearchText) ||
                ut.MaNguoiDungNavigation.Sdt.Contains(SearchText) ||
                ut.MaDonDat.Contains(SearchText)) && (ut.TrangThai == 0 || ut.TrangThai == 2))
                    .Include(s => s.MaNguoiDungNavigation)
                    .Include(s => s.DonDatCts)
                    .ToList();

            }
            else
            {
                items = _context.DonDats.Where(ut =>ut.TrangThai == 0 || ut.TrangThai == 2)
                   .Include(s => s.MaNguoiDungNavigation)
                   .ToList();
            }

            items = DoSort(items, SortProperty, sortOrder);

            PaginatedList<DonDat> retItems = new PaginatedList<DonDat>(items, pageIndex, pageSize);

            return retItems;
        }

        public DonDat GetItem(string id)
        {
            DonDat? item = _context.DonDats.Where(s => s.MaDonDat == id)
                .Include(s => s.MaNguoiDungNavigation)
                .Include(d => d.DonDatCts).ThenInclude(i => i.MaGiayNavigation)
                .FirstOrDefault();
            item.DonDatCts.ForEach(p => p.Description = p.MaGiayNavigation.TenGiay);
            item.DonDatCts.ForEach(p => p.Total = (decimal)(p.SoLuong * p.DonGia));
            item.TenKH = item.MaNguoiDungNavigation.HoNguoiDung + " " + item.MaNguoiDungNavigation.TenNguoiDung;
            item.Sdt = item.MaNguoiDungNavigation.Sdt;
            item.Email = item.MaNguoiDungNavigation.Email;
            var updateDonDat = _context.DonDats.Where(s=>s.MaDonDat == id).FirstOrDefault();
            updateDonDat.TrangThai = 2;
            _context.DonDats.Update(updateDonDat);
            _context.SaveChanges();
            return item;
        }

        public DonDat Delete(int id)
        {
            var getDonDat = _context.DonDats.Where(s => int.Parse(s.MaDonDat) == id).FirstOrDefault();
            var getDonDatCts = _context.DonDatCts.Where(s => int.Parse(s.MaDonDat) == id).ToList();
            foreach (var item in getDonDatCts)
            {
                var getKho = _context.KhoGiays.Where(s=>s.MaGiay == item.MaGiay && s.MaSize == item.MaSize).FirstOrDefault();
                getKho.SoLuong += item.SoLuong;
                _context.KhoGiays.Update(getKho);
                _context.SaveChanges();
            }
            getDonDat.TrangThai = 2;
            _context.DonDats.Update(getDonDat);
            _context.SaveChanges();
            return getDonDat;
        }

        public async Task<string> CreateHoaDon(DonDat item)
        {
            var items = _context.DonDats.Where(s => s.MaDonDat == item.MaDonDat).Include(s => s.DonDatCts).FirstOrDefault();
            var DonDatCT = _context.DonDatCts.Where(s => s.MaDonDat == items.MaDonDat).ToList();
            var HoaDon = new HoaDon
            {
                MaHd = item.MaDonDat,
                MaNguoiDung = items.MaNguoiDung,
                NgayLapDh = item.NgayDat,
                IdVNPay = item.MaVnpay,
                NgayGiaoHd = DateTime.Now,
                DiaChiNhan = item.DiaChiNhan,
                GhiChu =item.GhiChu
            };
            HoaDon.HoaDonCts = new List<HoaDonCt>();
            float tong = 0;
            foreach (var hoadon in DonDatCT)
            {
                HoaDon.HoaDonCts.Add(new HoaDonCt()
                {
                    MaHd = HoaDon.MaHd,
                    MaGiay = hoadon.MaGiay,
                    MaSize = hoadon.MaSize,
                    SoLuong = hoadon.SoLuong,
                    DonGia = hoadon.DonGia
                });
                tong += (float)(hoadon.SoLuong * hoadon.DonGia);
            }
            HoaDon.TongTien = tong;
            items.TrangThai = 1;
            await _context.HoaDons.AddAsync(HoaDon);
            _context.DonDats.Update(items);
            await _context.SaveChangesAsync();
            return HoaDon.MaHd;

        }

        public EmailModel GetMail(string MaHD)
        {
            var item = _context.DonDats.Where(s => s.MaDonDat == MaHD).Include(s => s.MaNguoiDungNavigation).FirstOrDefault();
            var sendEmail = new EmailModel
            {
                MaHoaDon = MaHD,
                DiaChi = item.DiaChiNhan,
                Email = item.MaNguoiDungNavigation.Email,
                Sdt = item.MaNguoiDungNavigation.Sdt.Trim(),
                HoTen = item.MaNguoiDungNavigation.HoNguoiDung + " " + item.MaNguoiDungNavigation.TenNguoiDung,
                NgayDat = (DateTime)item.NgayDat
            };
            var getProducts = _context.DonDatCts.Where(s => s.MaDonDat == MaHD).Include(s=>s.MaGiayNavigation).Include(s=>s.MaSizeNavigation).ToList();
            sendEmail.DonDatCts = getProducts;
            return sendEmail;
        }

        public int GetCountDonDat()
        {
            int count = _context.DonDats.Where(s=>s.TrangThai == 0).Count();
            return count;
        }

        public double GetTotalInCome()
        {
            DateTime StartDate = DateTime.Today.AddDays(-6);
            DateTime EndDate = DateTime.Now;
            var bill = _context.HoaDons.Where(s=>s.NgayLapDh >= StartDate && s.NgayLapDh <= EndDate).ToList();
            double? total = 0;
            foreach (var item in bill)
            {
                total += item.TongTien;
            }
            return (double)total;
        }
    }
}

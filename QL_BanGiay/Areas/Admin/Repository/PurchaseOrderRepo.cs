using Microsoft.EntityFrameworkCore;
using QL_BanGiay.Areas.Admin.Interface;
using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Data;
using QL_BanGiay.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QL_BanGiay.Areas.Admin.Repository
{
    public class PurchaseOrderRepo : IPurchaseOrder
    {
        private string _errors = "";
        private readonly QlyBanGiayContext _context;
        private readonly IWareHouse _WareHouseRepo;
        public PurchaseOrderRepo(QlyBanGiayContext context, IWareHouse WareHouseRepo)
        {
            _context = context;
            _WareHouseRepo = WareHouseRepo;
        }
        private List<NhapHang> DoSort(List<NhapHang> items, string SortProperty, SortOrder sortOrder)
        {

            if (SortProperty.ToLower() == "date")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderByDescending(n => n.NgayNhap).ToList();
                else
                    items = items.OrderBy(n => n.NgayNhap).ToList();
            }
            else if (SortProperty.ToLower() == "idorder")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.MaNhapHang).ToList();
                else
                    items = items.OrderByDescending(n => n.MaNhapHang).ToList();
            }
            else if (SortProperty.ToLower() == "name")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.MaDonViNhapNavigation.TenDonViNhap).ToList();
                else
                    items = items.OrderByDescending(n => n.MaDonViNhapNavigation.TenDonViNhap).ToList();
            }
            else if (SortProperty.ToLower() == "idbill")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.SoHoaDon).ToList();
                else
                    items = items.OrderByDescending(n => n.SoHoaDon).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderByDescending(d => d.NgayNhap).ToList();
                else
                    items = items.OrderBy(d => d.NgayNhap).ToList();
            }

            return items;
        }
        public async Task<NhapHang> Create(NhapHang nhaphang)
        {
            var items = nhaphang.NhapHangCts.ToArray();
            int total = 0;
            for (int i = 0; i < nhaphang.NhapHangCts.Count; i++)
            {
                bool checkShoeSize = _WareHouseRepo.IsShoeSizeNoExists(items[i].MaGiay, items[i].MaSize);
                if (checkShoeSize)
                {
                    var item = _WareHouseRepo.GetItemWareHouse(items[i].MaGiay, items[i].MaSize);
                    item.SoLuong += items[i].SoLuong;
                    _context.KhoGiays.Update(item);
                }
                else
                {
                    var newWareHouse = new KhoGiay
                    {
                        MaGiay = items[i].MaGiay,
                        MaSize = (int)items[i].MaSize,
                        SoLuong = items[i].SoLuong
                    };
                    _context.KhoGiays.Add(newWareHouse);
                }
                total += (int)(items[i].SoLuong * items[i].GiaMua);
                var shoe = _context.Giays.Where(s => s.MaGiay == items[i].MaGiay).FirstOrDefault();
                shoe.GiaBan = (int?)Math.Round((double)(items[i].GiaMua * 1.3));
                _context.Giays.Update(shoe);
            }
            nhaphang.TongTien = total;
            _context.Add(nhaphang);
            await _context.SaveChangesAsync();
            return nhaphang;
        }

        public bool Delete(int id)
        {
            bool retVal = false;
            _errors = "";

            try
            {
                var item = _context.NhapHangs.Where(s => s.MaNhapHang == id).FirstOrDefault();
                var nhapHangCT = _context.NhapHangCts.Where(s => s.MaNhapHang == id).ToList();
                foreach (var items in nhapHangCT)
                {
                    _context.Remove(items);
                    _context.SaveChanges();
                }
                _context.Remove(item);
                _context.SaveChanges();
                retVal = true;
            }
            catch (Exception ex)
            {
                _errors = "Delete Failed - Sql Exception Occured , Error Info : " + ex.Message;
            }
            return retVal;
        }

        public bool Edit(NhapHang nhapHang)
        {
            bool retVal = false;
            DateTime NgayNhap = Convert.ToDateTime(nhapHang.NgayNhap);
            DateTime NgayEidt = Convert.ToDateTime(DateTime.Now);
            TimeSpan Time = NgayEidt - NgayNhap;
            int soNgay = Time.Days;
            int total = 0;
            if (soNgay < 10)
            {
                List<NhapHangCt> nhapHangCts = _context.NhapHangCts.Where(s => s.MaNhapHang == nhapHang.MaNhapHang).ToList();
                for (int i = 0; i < nhapHangCts.Count; i++)
                {
                    var getWareHouse = _context.KhoGiays.Where(s => s.MaGiay == nhapHangCts[i].MaGiay && s.MaSize == nhapHangCts[i].MaSize).FirstOrDefault();

                    if (getWareHouse != null)
                    {
                        if (nhapHangCts[i].SoLuong < nhapHang.NhapHangCts[i].SoLuong)
                        {
                            getWareHouse.SoLuong += (nhapHang.NhapHangCts[i].SoLuong - nhapHangCts[i].SoLuong);
                            _context.KhoGiays.Update(getWareHouse);
                            _context.SaveChanges();
                        }
                        else if (nhapHangCts[i].SoLuong > nhapHang.NhapHangCts[i].SoLuong)
                        {
                            getWareHouse.SoLuong -= (nhapHangCts[i].SoLuong - nhapHang.NhapHangCts[i].SoLuong);
                            _context.KhoGiays.Update(getWareHouse);
                            _context.SaveChanges();
                        }
                    }
                    total += (int)(nhapHangCts[i].SoLuong * nhapHangCts[i].GiaMua);
                    var shoe = _context.Giays.Where(s => s.MaGiay == nhapHangCts[i].MaGiay).FirstOrDefault();
                    shoe.GiaBan = (int?)Math.Round((double)(nhapHangCts[i].GiaMua * 1.3));
                    _context.Giays.Update(shoe);
                }
                nhapHang.TongTien = total;
                _context.NhapHangCts.RemoveRange(nhapHangCts);
                _context.SaveChanges();
                _context.Attach(nhapHang);
                _context.Entry(nhapHang).State = EntityState.Modified;
                _context.NhapHangCts.AddRange(nhapHang.NhapHangCts);
                _context.SaveChanges();
                retVal = true;
            }
            return retVal;

        }

        public NhapHang GetItem(int id)
        {
            NhapHang? item = _context.NhapHangs.Where(i => i.MaNhapHang == id)
                .Include(d => d.NhapHangCts)
                .ThenInclude(i => i.MaGiayNavigation)
                .FirstOrDefault();
            item.NhapHangCts.ForEach(p => p.Description = p.MaGiayNavigation.TenGiay);
            item.NhapHangCts.ForEach(p => p.Total = (decimal)(p.SoLuong * p.GiaMua));
            return item;
        }

        public PaginatedList<NhapHang> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<NhapHang> items;
            if (SearchText != "" && SearchText != null)
            {
                items = _context.NhapHangs.Where(s=> s.MaNhapHang.ToString().Contains(SearchText)
                || s.SoHoaDon.ToString().Contains(SearchText)
                || s.MaDonViNhapNavigation.TenDonViNhap.ToLower().Contains(SearchText.ToLower()))
                    .Include(s => s.MaDonViNhapNavigation).ToList();
            }
            else
                items = _context.NhapHangs.Include(s => s.MaDonViNhapNavigation).ToList();
            items = DoSort(items, SortProperty, sortOrder);

            PaginatedList<NhapHang> retItems = new PaginatedList<NhapHang>(items, pageIndex, pageSize);

            return retItems;
        }

        public int GetNewPONumber()
        {
            int number = 0;
            var LastNumber = _context.NhapHangs.Max(s => s.MaNhapHang);
            if (LastNumber == 0)
            {
                number = 1;
            }
            else
            {
                number = LastNumber + 1;
            }
            return number;
        }

        public string GetErrors()
        {
            return _errors;
        }

        public int? GetTotalExprense()
        {
            DateTime StartDate = DateTime.Today.AddDays(-6);
            DateTime EndDate = DateTime.Now;
            var get = _context.NhapHangs.Where(s=>s.NgayNhap >= StartDate && s.NgayNhap <= EndDate).ToList();
            int? total = 0;
            foreach (var item in get)
            {
                total += item.TongTien;
            }
            return total;
        }

        public bool IsHoaDonNoExits(int? id)
        {
            var check = _context.NhapHangs.Where(s => s.SoHoaDon == id).Count();
            if (check > 0) return true; else return false;
        }
    }
}

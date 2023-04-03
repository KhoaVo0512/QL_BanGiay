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

            if (SortProperty.ToLower() == "SuppliersId")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.MaDonViNhap).ToList();
                else
                    items = items.OrderByDescending(n => n.MaDonViNhap).ToList();
            }
            else if (SortProperty.ToLower() == "OrderId")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.SoHoaDon).ToList();
                else
                    items = items.OrderByDescending(n => n.SoHoaDon).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderByDescending(d => d.MaNhapHang).ToList();
                else
                    items = items.OrderBy(d => d.MaNhapHang).ToList();
            }

            return items;
        }
        public async Task<NhapHang> Create(NhapHang nhaphang)
        {
            var items = nhaphang.NhapHangCts.ToArray();
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
            }
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
                foreach(var items in nhapHangCT)
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
            List<NhapHangCt> nhapHangCts = _context.NhapHangCts.Where(s => s.MaNhapHang == nhapHang.MaNhapHang).ToList();
            _context.NhapHangCts.RemoveRange(nhapHangCts);
            _context.SaveChanges();
            _context.Attach(nhapHang);
            _context.Entry(nhapHang).State = EntityState.Modified;
            _context.NhapHangCts.AddRange(nhapHang.NhapHangCts);
            _context.SaveChanges();
            retVal = true;
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
                items = _context.NhapHangs.ToList();
            }
            else
                items = _context.NhapHangs.ToList();
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
            }else
            {
                number = LastNumber + 1;
            }
            return number;
        }

        public string GetErrors()
        {
            return _errors;
        }
    }
}

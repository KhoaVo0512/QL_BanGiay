using Microsoft.EntityFrameworkCore;
using QL_BanGiay.Areas.Admin.Interface;
using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Data;
using QL_BanGiay.Models;

namespace QL_BanGiay.Areas.Admin.Repository
{
    public class CollectionRepo : ICollection
    {
        private readonly QlyBanGiayContext _context;
       
        public CollectionRepo(QlyBanGiayContext context)
        {
            _context = context;
        }
        private List<DongSanPham> DoSort(List<DongSanPham> items, string SortProperty, SortOrder sortOrder)
        {

            if (SortProperty.ToLower() == "namecollection")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.TenDongSanPham).ToList();
                else
                    items = items.OrderByDescending(n => n.TenDongSanPham).ToList();
            }
            else if (SortProperty.ToLower() == "namebrand")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.MaNhanHieuNavigation.TenNhanHieu).ToList();
                else
                    items = items.OrderByDescending(n => n.MaNhanHieuNavigation.TenNhanHieu).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(d => d.TenDongSanPham).ToList();
                else
                    items = items.OrderByDescending(d => d.TenDongSanPham).ToList();
            }

            return items;
        }
        public PaginatedList<DongSanPham> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<DongSanPham> items;
            if (SearchText != null && SearchText != "")
            {
                items = _context.DongSanPhams.Where(ut => ut.TenDongSanPham.Contains(SearchText.ToLower())
                || ut.MaNhanHieuNavigation.TenNhanHieu.ToLower().Contains(SearchText.ToLower())).Include(s=>s.MaNhanHieuNavigation).ToList();
            }
            else
            {
                items = _context.DongSanPhams.Include(s => s.MaNhanHieuNavigation).ToList();
            }
            items = DoSort(items, SortProperty, sortOrder);
            PaginatedList<DongSanPham> retItems = new PaginatedList<DongSanPham>(items, pageIndex, pageSize);
            return retItems;
        }

        public async Task<DongSanPham> Create(DongSanPham dongsanpham)
        {
            dongsanpham.TenDongSanPham.Trim();
            _context.Add(dongsanpham);
            await _context.SaveChangesAsync();
            return dongsanpham;
        }

        public DongSanPham GetItem(int? id)
        {
            DongSanPham item = _context.DongSanPhams.Where(s => s.MaDongSanPham == id).FirstOrDefault();
            return item;
        }

        public async Task<DongSanPham> Edit(DongSanPham dongsanpham)
        {

            var item = _context.DongSanPhams.Where(s=>s.MaDongSanPham == dongsanpham.MaDongSanPham).FirstOrDefault();
            item.MaNhanHieu = dongsanpham.MaNhanHieu;
            item.TenDongSanPham = dongsanpham.TenDongSanPham;
            await _context.SaveChangesAsync();
            return dongsanpham;
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var shoe = _context.Giays.Where(s=>s.MaDongSanPham == id).FirstOrDefault();
                if (shoe == null)
                {
                    var collection = await _context.DongSanPhams.FindAsync(id);

                    _context.Remove(collection);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                    return false;
               
            }catch (Exception ex)
            {
                return false;
            }
           
            
        }

        public List<DongSanPham> GetCollections(int id)
        {
            var lstCollection = _context.DongSanPhams.OrderBy(e=>e.TenDongSanPham).Where(e=>e.MaNhanHieu == id).ToList();
            return lstCollection;
        }

        public List<DongSanPham> GetCollectionAdidas()
        {
            var items = _context.DongSanPhams.Where(s=>s.MaNhanHieuNavigation.MaNhanHieu == 3).OrderBy(s=>s.TenDongSanPham).ToList();
            return items;
        }

        public List<DongSanPham> GetCollectionConverse()
        {
            var items = _context.DongSanPhams.Where(s => s.MaNhanHieuNavigation.MaNhanHieu == 1).OrderBy(s => s.TenDongSanPham).ToList();
            return items;
        }

        public List<DongSanPham> GetCollectionNike()
        {
            var items = _context.DongSanPhams.Where(s => s.MaNhanHieuNavigation.MaNhanHieu == 4).OrderBy(s => s.TenDongSanPham).ToList();
            return items;
        }

        public List<DongSanPham> GetCollectionVans()
        {
            var items = _context.DongSanPhams.Where(s => s.MaNhanHieuNavigation.MaNhanHieu == 2).OrderBy(s => s.TenDongSanPham).ToList();
            return items;
        }
    }
}

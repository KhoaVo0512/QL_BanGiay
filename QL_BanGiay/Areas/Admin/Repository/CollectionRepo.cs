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
        private List<CollectionModel> DoSort(List<CollectionModel> items, string SortProperty, SortOrder sortOrder)
        {

            if (SortProperty.ToLower() == "namecollection")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.DongSanPham.TenDongSanPham).ToList();
                else
                    items = items.OrderByDescending(n => n.DongSanPham.TenDongSanPham).ToList();
            }
            else if (SortProperty.ToLower() == "namebrand")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.nhanHieu.TenNhanHieu).ToList();
                else
                    items = items.OrderByDescending(n => n.nhanHieu.TenNhanHieu).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderByDescending(d => d.DongSanPham.MaDongSanPham).ToList();
                else
                    items = items.OrderBy(d => d.DongSanPham.MaDongSanPham).ToList();
            }

            return items;
        }
        public PaginatedList<CollectionModel> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<NhanHieu> nhanHieus = _context.NhanHieus.ToList();
            List<DongSanPham> dongSanPhams = _context.DongSanPhams.ToList();
            List<CollectionModel> collectionModels;
            if (SearchText != null && SearchText != "")
            {
                dongSanPhams = _context.DongSanPhams.Where(s => s.TenDongSanPham.Contains(SearchText)).ToList();
                collectionModels = (from g in dongSanPhams
                                    join b in nhanHieus on g.MaNhanHieu equals b.MaNhanHieu into table1
                                    from t in table1.DefaultIfEmpty()
                                    select new CollectionModel
                                    {
                                        DongSanPham = g,
                                        nhanHieu = t

                                    }).ToList();
            }
            else
            {
                collectionModels = (from g in dongSanPhams
                                    join b in nhanHieus on g.MaNhanHieu equals b.MaNhanHieu into table1
                                    from t in table1.DefaultIfEmpty()
                                    select new CollectionModel
                                    {
                                        DongSanPham = g,
                                        nhanHieu = t

                                    }).ToList();
            }
            collectionModels = DoSort(collectionModels, SortProperty, sortOrder);
            PaginatedList<CollectionModel> retItems = new PaginatedList<CollectionModel>(collectionModels, pageIndex, pageSize);
            return retItems;
        }

        public async Task<DongSanPham> Create(DongSanPham dongsanpham)
        {
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

            var item = _context.DongSanPhams.Where(s=>s.MaNhanHieu == dongsanpham.MaNhanHieu).FirstOrDefault();
            item.TenDongSanPham = dongsanpham.TenDongSanPham;
            await _context.SaveChangesAsync();
            return dongsanpham;
        }

        public async Task<int> Delete(int id)
        {
            var collection = await _context.DongSanPhams.FindAsync(id);
            _context.Remove(collection);
             await _context.SaveChangesAsync();
            return collection.MaDongSanPham;
        }
    }
}

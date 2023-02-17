
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using NuGet.Frameworks;
using QL_BanGiay.Areas.Admin.Interface;
using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Data;


namespace QL_BanGiay.Areas.Admin.Repository
{
    public class ShoeRepo : IShoe
    {
        private readonly QlyBanGiayContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ShoeRepo(QlyBanGiayContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        private List<Giay> DoSort(List<Giay> items, string SortProperty, SortOrder sortOrder)
        {

            if (SortProperty.ToLower() == "nameshoe")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.TenGiay).ToList();
                else
                    items = items.OrderByDescending(n => n.TenGiay).ToList();
            }
            else if (SortProperty.ToLower() == "magiay")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.MaGiay).ToList();
                else
                    items = items.OrderByDescending(n => n.MaGiay).ToList();
            }else if (SortProperty.ToLower() == "price")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.GiaBan).ToList();
                else
                    items = items.OrderByDescending(n => n.GiaBan).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderByDescending(d => d.MaGiay).ToList();
                else
                    items = items.OrderBy(d => d.MaGiay).ToList();
            }

            return items;
        }
        public PaginatedList<Giay> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<Giay> items;
            if (SearchText != "" && SearchText != null)
            {
                items = _context.Giays.Where(ut=>ut.TenGiay.Contains(SearchText))
                    .Include(s=>s.MaDongSanPhamNavigation)
                    .Include(s=>s.MaNhaSanXuatNavigation)
                    .ToList();
            }
            else
                items = _context.Giays
                    .Include(s => s.MaDongSanPhamNavigation)
                    .Include(s => s.MaNhaSanXuatNavigation)
                    .ToList();
            items = DoSort(items, SortProperty, sortOrder);

            PaginatedList<Giay> retItems = new PaginatedList<Giay>(items, pageIndex, pageSize);

            return retItems;
        }

        public async Task<ShoeContext> Create(ShoeContext item)
        {
            string folder;
            if (item.MaAnhNen != null)
            {
                folder = "assets/images/anhdaidien/";
                item.anhNenUrl = await UploadImage(folder, item.MaAnhNen);
            }
            if (item.AnhDetail != null)
            {
                if (item.MaNhanHieu == 1)
                    folder = "assets/images/converse/";
                else if (item.MaNhanHieu == 2)
                    folder = "assets/images/vans/";
                else if (item.MaNhanHieu == 3)
                    folder = "assets/images/adidas/";
                else folder = "assets/images/nike/";
                item.Images = new List<ShoeImageContext>();
                foreach (var file in item.AnhDetail)
                {
                    var shoeImage = new ShoeImageContext()
                    {
                        Name = file.FileName,
                        URL = await UploadImage(folder, file)                    
                    };
                item.Images.Add(shoeImage);
                }
            }
            var newShoe = new Giay
            {
                MaGiay = item.MaGiay.Trim(),
                MaDongSanPham = item.MaDongSanPham,
                MauSac = item.MauSac,
                ChatLieu = item.ChatLieu,
                TenGiay = item.TenGiay,
                AnhDD = item.anhNenUrl,
                TrangThai = true,
                NgayCn = DateTime.Now,
                MaNhaSanXuat = item.MaNhaSanXuat
            };
            newShoe.AnhGiays = new List<AnhGiay>();
            foreach(var file in item.Images)
            {
                newShoe.AnhGiays.Add(new AnhGiay()
                {
                    TenAnh = file.Name,
                    Url = file.URL
                });
            }
            await _context.Giays.AddAsync(newShoe);
            await _context.SaveChangesAsync();
            return item;
        }

        public bool IsShoeNoExists(string magiay)
        {
            int ct = _context.Giays.Where(e => e.MaGiay.ToLower() == magiay.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
        private async Task<string> UploadImage(string folderPath, IFormFile file)
        {

            folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;

            string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);

            using (var iNeedToLearnAboutDispose = new FileStream(serverFolder, FileMode.Create))
            {
                await file.CopyToAsync(iNeedToLearnAboutDispose);
            }
            return "/" + folderPath;
        }

        public ShoeContext GetItem(string id)
        {
            var getShoe = _context.Giays.Where(s => s.MaGiay == id).Include(e => e.AnhGiays).FirstOrDefault();
            var getBrand = _context.DongSanPhams.Where(s => s.MaDongSanPham == getShoe.MaDongSanPham).Select(s => s.MaNhanHieu).SingleOrDefault();

            ShoeContext item = new ShoeContext
            {
                MaGiay = getShoe.MaGiay,
                TenGiay = getShoe.TenGiay,
                MaDongSanPham = (int)getShoe.MaDongSanPham,
                ChatLieu = getShoe.ChatLieu,
                MauSac = getShoe.MauSac,
                MaNhaSanXuat = (int)getShoe.MaNhaSanXuat,
                MaNhanHieu = (int)getBrand,
            };
            return item;
        }

        public async Task<ShoeContext> Edit(ShoeContext item)
        {
            var getShoe = _context.Giays.Where(s => s.MaGiay == item.MaGiay).FirstOrDefault();
            string serverFolder = _webHostEnvironment + getShoe.AnhDD.ToString();
            FileInfo fi = new FileInfo(serverFolder);
            if (fi != null)
                File.Delete(serverFolder);
            string folder;
            if (item.MaAnhNen != null)
            {
                folder = "assets/images/anhdaidien/";
                item.anhNenUrl = await UploadImage(folder, item.MaAnhNen);
            }
            if (item.AnhDetail != null)
            {
                if (item.MaNhanHieu == 1)
                    folder = "assets/images/converse/";
                else if (item.MaNhanHieu == 2)
                    folder = "assets/images/vans/";
                else if (item.MaNhanHieu == 3)
                    folder = "assets/images/adidas/";
                else folder = "assets/images/nike/";
                item.Images = new List<ShoeImageContext>();
                foreach (var file in item.AnhDetail)
                {
                    var shoeImage = new ShoeImageContext()
                    {
                        Name = file.FileName,
                        URL = await UploadImage(folder, file)
                    };
                    item.Images.Add(shoeImage);
                }
            }
            getShoe.TenGiay = item.TenGiay;
            getShoe.NgayCn = DateTime.Now;
            getShoe.ChatLieu = item.ChatLieu;
            getShoe.MauSac = item.MauSac;
            getShoe.MaNhaSanXuat = item.MaNhaSanXuat;
            getShoe.MaDongSanPham = item.MaDongSanPham;
            getShoe.AnhDD = item.anhNenUrl;
            var getImageShoe = _context.AnhGiays.Where(s => s.MaGiay == item.MaGiay).ToList();
            var id = item.Images.ToArray();
            int i = 0;
            var path = "";
            foreach (var items in getImageShoe)
            {
                path = _webHostEnvironment.WebRootPath + items.Url.ToString();
                FileInfo fa = new FileInfo(path);
                if (fa!= null)
                {
                    File.Delete(path);
                    fa.Delete();
                }
                items.TenAnh = id[i].Name;
                items.Url = id[i].URL;
                i++;
                _context.AnhGiays.Update(items);
            }
            _context.Update(getShoe);
            await _context.SaveChangesAsync();
            return item;
        }

        public bool Delete(string id)
        {
            var getShoe = _context.Giays.Where(s => s.MaGiay == id).FirstOrDefault();
            getShoe.TrangThai = false;
            _context.Update(getShoe);
            _context.SaveChanges();
            return true;
        }

        public List<Giay> GetItemsVans()
        {
            var items = (from g in _context.Giays
                         join b in _context.DongSanPhams on g.MaDongSanPham equals b.MaDongSanPham
                         where b.MaNhanHieu == 2
                         where g.TrangThai == true
                         select g).ToList();
            return items;
        }

        public List<Giay> GetItemsConverse()
        {
            var items = (from g in _context.Giays
                         join b in _context.DongSanPhams on g.MaDongSanPham equals b.MaDongSanPham
                         where b.MaNhanHieu == 1
                         where g.TrangThai == true
                         select g).ToList();
            return items;
        }

        public List<Giay> GetItemsNike()
        {
            var items = (from g in _context.Giays
                         join b in _context.DongSanPhams on g.MaDongSanPham equals b.MaDongSanPham
                         where b.MaNhanHieu == 4
                         where g.TrangThai == true
                         select g).ToList();
            return items;
        }

        public List<Giay> GetItemsAdidas()
        {
            var items = (from g in _context.Giays
                         join b in _context.DongSanPhams on g.MaDongSanPham equals b.MaDongSanPham
                         where b.MaNhanHieu == 3
                         where g.TrangThai == true
                         select g).ToList();
            return items;
        }
    }
}

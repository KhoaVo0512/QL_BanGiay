using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using QL_BanGiay.Areas.Admin.Interface;
using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Data;

namespace QL_BanGiay.Areas.Admin.Repository
{
    public class ShoeDetailsRepo : IShoeDetails
    {
        private readonly QlyBanGiayContext _context;
        public ShoeDetailsRepo(QlyBanGiayContext context)
        {
            _context = context;
        }

        public bool Edit(ShoeDetails context)
        {
            try
            {
                var item = _context.Giays.Where(s => s.MaGiay == context.MaGiay).FirstOrDefault();
                if (item != null)
                {
                    item.NoiDung = context.content;
                    item.TomTat = context.tomtat;
                    _context.Update(item);
                    _context.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public ShoeDetails GetItem(string shoeId)
        {
            var getShoe = _context.Giays.Where(s => s.MaGiay == shoeId)
                .Include(e => e.AnhGiays)
                .FirstOrDefault();
            ShoeDetails item = new ShoeDetails();
            if (getShoe.NoiDung == null)
            {
                item.MaGiay = getShoe.MaGiay;
                item.TenGiay = getShoe.TenGiay;
                item.content = "";
                item.tomtat = "";
            }else
            {
                item.MaGiay = getShoe.MaGiay;
                item.TenGiay = getShoe.TenGiay;
                item.content = getShoe.NoiDung;
                item.tomtat = getShoe.TomTat;
            }
            return item;
        }
    }
}

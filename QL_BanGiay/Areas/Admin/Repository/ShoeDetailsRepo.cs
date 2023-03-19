using Microsoft.EntityFrameworkCore;
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
                item.NoiDungs.Add(new NoiDung()
                {
                    MaGiay = context.MaGiay,
                    ThongTin = context.content
                });
                _context.Update(item);
                _context.SaveChanges();
                return true;
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
            var getContent = _context.NoiDungs.Where(s => s.MaGiay == shoeId).FirstOrDefault();
            ShoeDetails item = new ShoeDetails();
            if (getContent == null)
            {
                item.MaGiay = getShoe.MaGiay.ToString();
                item.TenGiay = getShoe.TenGiay;
                item.content = "";
            }
            else
            {
                item.MaGiay = getShoe.MaGiay.ToString();
                item.TenGiay = getShoe.TenGiay;
                item.content = getContent.ThongTin;
            }
            return item;
        }
    }
}

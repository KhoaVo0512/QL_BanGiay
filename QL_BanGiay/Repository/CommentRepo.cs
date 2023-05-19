using QL_BanGiay.Data;
using QL_BanGiay.Interface;

namespace QL_BanGiay.Repository
{
    public class CommentRepo : IComment
    {
        private readonly QlyBanGiayContext _context;
        public CommentRepo(QlyBanGiayContext context)
        {
            _context = context;
        }
        public bool CreateComment(string maGiay, int stars, string comment, string name)
        {
            try
            {
                var danhgia = new DanhGium
                {
                    MaGiay = maGiay,
                    Sao = stars,
                    NoiDung = comment,
                    HoTen = name,
                    NgayTao = DateTime.Now
                };
                _context.DanhGia.Update(danhgia);
                _context.SaveChanges();
                return true;
            }catch (Exception ex)
            {
                return false;
            }

        }

        public List<DanhGium> GetComment(string shoeId)
        {
            var items = _context.DanhGia.Where(s=>s.MaGiay == shoeId).OrderByDescending(a=>a.NgayTao).ToList();
            return items;
        }

        public int getCommentLength(string shoeId)
        {
            var length = _context.DanhGia.Where(s=>s.MaGiay == shoeId).Count();
            return length;
        }

        public int getPercent1Star(string shoeId)
        {
            var star = _context.DanhGia.Where(s => s.MaGiay == shoeId && s.Sao == 1).Count();
            var count = getCommentLength(shoeId);
            float pecent;
            if (star > 0)
            {
                pecent = ((float)star / count) * 100;
                return (int)pecent;
            }
            else
                return 0;
        }

        public int getPercent2Star(string shoeId)
        {
            var star = _context.DanhGia.Where(s=>s.MaGiay == shoeId && s.Sao == 2).Count();
            var count = getCommentLength(shoeId);
            float pecent;
            if (star > 0)
            {
                pecent = ((float)star / count) * 100;
                return (int)pecent;
            }
            else
                return 0;

        }

        public int getPercent3Star(string shoeId)
        {
            var star = _context.DanhGia.Where(s => s.MaGiay == shoeId && s.Sao == 3).Count();
            var count = getCommentLength(shoeId);
            float pecent;
            if (star > 0)
            {
                pecent = ((float)star / count) * 100;
                return (int)pecent;
            }
            else
                return 0;
        }

        public int getPercent4Star(string shoeId)
        {
            var star = _context.DanhGia.Where(s => s.MaGiay == shoeId && s.Sao == 4).Count();
            var count = getCommentLength(shoeId);
            float pecent;
            if (star > 0)
            {
                pecent = ((float)star / count) * 100;
                return (int)pecent;
            }
            else
                return 0;
        }

        public int getPercent5Star(string shoeId)
        {
            var star = _context.DanhGia.Where(s => s.MaGiay == shoeId && s.Sao == 5).Count();
            var count = getCommentLength(shoeId);
            float pecent;
            if (star > 0)
            {
                pecent = ((float)star / count) * 100;
                return (int)pecent;
            }
            else
                return 0;
        }

        public int getStars(string shoeId)
        {
            var stars = _context.DanhGia.Where(s=>s.MaGiay == shoeId).ToList();
            var count = 0;
            foreach (var s in stars)
            {
                count +=(int) s.Sao;
            }
            return count;
        }
    }
}

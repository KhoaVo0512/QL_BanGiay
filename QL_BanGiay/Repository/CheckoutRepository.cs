using Microsoft.EntityFrameworkCore;
using QL_BanGiay.Areas.Admin.Interface;
using QL_BanGiay.Data;
using QL_BanGiay.Interface;
using QL_BanGiay.Models;
using System.Text;

namespace QL_BanGiay.Repository
{
    public class CheckoutRepository : ICheckout
    {
        private readonly QlyBanGiayContext _context;
        private readonly IAddress _AddressRepo;
        public CheckoutRepository(QlyBanGiayContext context, IAddress address)
        {
            _context = context;
            _AddressRepo = address;
        }

        public async Task<int> CheckProduct(string id, string idSize)
        {
            var item = await _context.KhoGiays.Where(s => s.MaGiay == id && s.MaSize == int.Parse(idSize)).FirstOrDefaultAsync();
            return Convert.ToInt32(item.SoLuong);
        }

        public DonDat CreateCheckout(ProductModel[] Stock, string id)
        {
           
            var getDonDat = _context.DonDats.Where(s => s.MaNguoiDung == id).FirstOrDefault();
            int total = 0;
            getDonDat.DonDatCts = new List<DonDatCt>();
            for (int i = 0; i < Stock.Length; i++)
            {
                getDonDat.DonDatCts.Add(new DonDatCt()
                {
                    MaGiay = Stock[i].Id,
                    MaSize = int.Parse(Stock[i].Idsize),
                    SoLuong = int.Parse(Stock[i].Quantity),
                    DonGia = (Stock[i].Total / int.Parse(Stock[i].Quantity))
                });
                total += Stock[i].Total;
                var getWareHouse = _context.KhoGiays.Where(s=>s.MaGiay == Stock[i].Id && s.MaSize == int.Parse(Stock[i].Idsize)).FirstOrDefault();
                getWareHouse.SoLuong -= int.Parse(Stock[i].Quantity);
                _context.KhoGiays.Update(getWareHouse);
                _context.SaveChanges();
            }
            _context.DonDats.Update(getDonDat);
            _context.SaveChanges();
            getDonDat.Total = total;
            return getDonDat;
        }

        public DonDat CreateCheckoutUser(CheckOutUserModel model, ProductModel[] items, string id, string IdVNpay, string note)
        {
            int length = 10;

            // creating a StringBuilder object()
            StringBuilder str_build = new StringBuilder();
            Random random = new Random();

            char letter;

            for (int i = 0; i < length; i++)
            {
                double flt = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(10 * flt));
                letter = Convert.ToChar(shift + 48);
                str_build.Append(letter);
            }
            var madd = str_build.ToString();
            string getAddress = "";
            int total = 0;
            if (model.IdDiaChi != null)
            {
                getAddress = _AddressRepo.GetFullAddress(int.Parse(model.IdDiaChi));
            }else
            {
                var diachi = new DiaChi{
                    MaNguoiDung = id,
                    MaXa = model.MaXa,
                    TenDiaChi = model.DiaChi
                };
                var tenDiaChi = _context.Xas.Where(s => s.MaXa == model.MaXa).Include(s => s.MaHuyenNavigation).ThenInclude(s => s.MaTinhNavigation).FirstOrDefault();
                getAddress = model.DiaChi + ", " + tenDiaChi.TenXa + ", " + tenDiaChi.MaHuyenNavigation.TenHuyen + ", " + tenDiaChi.MaHuyenNavigation.MaTinhNavigation.TenTinh + ".";
                _context.DiaChis.Add(diachi);
                _context.SaveChanges();
            }
            var dondat = new DonDat
            {
                MaDonDat = madd,
                MaNguoiDung = id,
                NgayDat = DateTime.Now,
                TrangThai = 0,
                GhiChu = note,
                MaVnpay = IdVNpay,
                DiaChiNhan = getAddress,
                DaThanhToan = false
            };
            dondat.DonDatCts = new List<DonDatCt>();
            for (int i = 0; i < items.Length; i++)
            {
                dondat.DonDatCts.Add(new DonDatCt()
                {
                    MaGiay = items[i].Id,
                    MaSize = int.Parse(items[i].Idsize),
                    SoLuong = int.Parse(items[i].Quantity),
                    DonGia = (items[i].Total / int.Parse(items[i].Quantity))
                });
                total += items[i].Total;
                var getKho = _context.KhoGiays.Where(s => s.MaGiay == items[i].Id && s.MaSize == int.Parse(items[i].Idsize)).FirstOrDefault();
                getKho.SoLuong -= int.Parse(items[i].Quantity);
                _context.KhoGiays.Update(getKho);
                _context.SaveChanges();
            }
           
            _context.DonDats.Add(dondat);
            _context.SaveChanges();
            dondat.Total = total;
            return dondat;
        }

        public async Task<bool> DeleteNguoiDungAndDonDat(string? idDonDat, string idNguoiDung)
        {
            var itemND = _context.NguoiDungs.Where(s=>s.MaNguoiDung == idNguoiDung).FirstOrDefault();   
            var itemDD = _context.DonDats.Where(s=> s.MaDonDat == idDonDat).FirstOrDefault();
            var itemDDct = _context.DonDatCts.Where(s=> s.MaDonDat == idDonDat).ToList();
            foreach (var item in itemDDct )
            {
                _context.Remove(item);
                await _context.SaveChangesAsync();
            }
            _context.DonDats.Remove(itemDD);
            _context.NguoiDungs.Remove(itemND);
            await _context.SaveChangesAsync();
            return true;
        }

        public bool EditCheckOut(string? id, long IdVNPay)
        {
            try
            {
                var item = _context.DonDats.Where(s => s.MaDonDat == id).Include(s=>s.DonDatCts).FirstOrDefault();
                item.DaThanhToan = true;
                item.MaVnpay = IdVNPay.ToString();
                _context.DonDats.Update(item);
                _context.SaveChanges();
                return true;
            }catch (Exception ex)
            {
                return false;
            }
        }
    }
}

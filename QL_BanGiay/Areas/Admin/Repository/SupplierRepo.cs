using NuGet.Common;
using QL_BanGiay.Areas.Admin.Interface;
using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Data;

namespace QL_BanGiay.Areas.Admin.Repository
{
    public class SupplierRepo : ISupplier
    {
        private readonly QlyBanGiayContext _context;
        public SupplierRepo (QlyBanGiayContext context) 
        {
            _context = context;
        }
        private List<DonViNhapHang> DoSort(List<DonViNhapHang> items, string SortProperty, SortOrder sortOrder)
        {

            if (SortProperty.ToLower() == "namesupplier")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.TenDonViNhap).ToList();
                else
                    items = items.OrderByDescending(n => n.TenDonViNhap).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderByDescending(d => d.MaDonViNhap).ToList();
                else
                    items = items.OrderBy(d => d.MaDonViNhap).ToList();
            }

            return items;
        }
        public PaginatedList<DonViNhapHang> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<DonViNhapHang> items;
            if (SearchText != "" && SearchText != null)
            {
                items = _context.DonViNhapHangs.Where(ut=>ut.TenDonViNhap.Contains(SearchText)).ToList();
            }
            else
                items = _context.DonViNhapHangs.ToList();
            items = DoSort(items, SortProperty, sortOrder);

            PaginatedList<DonViNhapHang> retItems = new PaginatedList<DonViNhapHang>(items, pageIndex, pageSize);

            return retItems;
        }
    }
}

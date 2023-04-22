using QL_BanGiay.Areas.Admin.Interface;
using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Data;

namespace QL_BanGiay.Areas.Admin.Repository
{
    public class RoleRepo : IRole
    {
        private readonly QlyBanGiayContext _context;
        public RoleRepo(QlyBanGiayContext context)
        {
            _context = context;
        }
        private List<Quyen> DoSort(List<Quyen> items, string SortProperty, SortOrder sortOrder)
        {

            if (SortProperty.ToLower() == "namerole")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.TenQuyen).ToList();
                else
                    items = items.OrderByDescending(n => n.TenQuyen).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(d => d.MaQuyen).ToList();
                else
                    items = items.OrderByDescending(d => d.TenQuyen).ToList();
            }

            return items;
        }
        public PaginatedList<Quyen> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            var items = _context.Quyens.ToList();
            items = DoSort(items, SortProperty, sortOrder);
            PaginatedList<Quyen> retItems = new PaginatedList<Quyen>(items, pageIndex, pageSize);
            return retItems;
        }

        public bool IsInRole(string id, string username)
        {
            var role = _context.QuyenCts.Where(s => s.UserName == username && s.MaQuyen.ToString() == id).Count();
            if (role > 0)
                return true;
            else
                return false;
        }

        public bool UpdateRepo(string username, UserRoleModel model)
        {
            try
            {
                var roles = _context.QuyenCts.Where(s => s.UserName == username).ToList();
                for (int i = 0; i < model.manageUserRoles.Count; i++)
                {
                    foreach (var item in roles)
                    {
                        if (item.UserName == username && model.manageUserRoles[i].RoleId == item.MaQuyen.ToString())
                        {
                            if (!model.manageUserRoles[i].Selected)
                            {
                                _context.QuyenCts.Remove(item);
                                _context.SaveChanges();
                            }
                        }
                        else if (model.manageUserRoles[i].Selected)
                        {
                            if (!IsInRole(model.manageUserRoles[i].RoleId, username))
                            {
                                var createRole = new QuyenCt
                                {
                                    MaQuyen = Convert.ToInt32(model.manageUserRoles[i].RoleId),
                                    UserName = username
                                };
                                _context.QuyenCts.Add(createRole);
                                _context.SaveChanges();
                            }
                        }
                    }
                   
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }


        }
    }
}

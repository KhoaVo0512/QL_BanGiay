using Microsoft.AspNetCore.Mvc;
using QL_BanGiay.Areas.Admin.Interface;

namespace QL_BanGiay.Controllers
{
    public class ConverseController : Controller
    {
        private readonly ICollection _CollectionRepo;
        public ConverseController (ICollection collectionRepo)
        {
            _CollectionRepo = collectionRepo;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetCollection()
        {
            var items = _CollectionRepo.GetCollectionConverse();
            return new JsonResult(items);
        }
    }
}

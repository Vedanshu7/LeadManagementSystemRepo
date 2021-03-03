using LMS.Common;
using LMS.Web.Attributes;
using LMS.Web.BAL.Interface;
using LMS.Web.BAL.ViewModels;

using System.Web.Mvc;

namespace LMS.Web.Controllers
{
    [Authenticate]
    [Authorization(RolesEnum.Admin)]
    public class AdminController : Controller
    {
        private readonly IBrandManager _brandManager;
        public AdminController(IBrandManager brandManager)
        {
            _brandManager = brandManager;
        }
        // GET: Admin
        public ActionResult Index()
        {
            return Content("This is Admin");
        }

        public ActionResult CreateBrand()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateBrand(AdminBrandViewModel model)
        {
            int loggedInUserId = (int)Session["loggedInId"];
          
            var result = _brandManager.CreateBrand(model,loggedInUserId);
            if(result=="Success")
            {
                TempData["NotificationSuccess"] = result;
                return RedirectToAction("BrandList", "Admin");
            }
            else
            {
                TempData["NotificationInfo"] = result;
                return View();
            }
          
        }

        [HttpGet]
        public ActionResult BrandList()
        {
          // return Content("Added.");
            var brands = _brandManager.GetBrandList();
            return View(brands);
        }
        [HttpPost]
        public ActionResult BrandList(AdminBrandViewModel model)
        {
            return View();
        }

        [HttpGet]
        public ActionResult EditBrand(int id)
        {           
            AdminBrandViewModel brand = _brandManager.GetBrand(id);
            return View(brand);
        }
        [HttpPost]
        public ActionResult EditBrand(AdminBrandViewModel model)
        {
            int loggedInUserId = (int)Session["loggedInId"];
            var result = _brandManager.EditBrand(model,loggedInUserId);
            if (result == "Success")
            {
                TempData["NotificationSuccess"] = result;
                return RedirectToAction("BrandList", "Admin");
            }
            else
            {
                TempData["NotificationInfo"] = result;
                return View();
            }
        }
    }
}

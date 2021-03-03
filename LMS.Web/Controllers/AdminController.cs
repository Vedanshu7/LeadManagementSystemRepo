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
        private readonly IModelManager _modelManager;
        public AdminController(IModelManager modelManager, IBrandManager brandManager)
        {
            _modelManager = modelManager;
            _brandManager = brandManager;
        }
        // GET: Admin
        public ActionResult Index()
        {
            return RedirectToAction("ListModel");
        }

        [HttpGet]
        public ActionResult CreateModel()
        {
            ViewBag.BrandId = new SelectList(_modelManager.GetBrandsDropDown(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult CreateModel(AdminModelViewModel model)
        {
            if (ModelState.IsValid)
            {
                int loggedInUserId = (int)Session["loggedInId"];
                var result = _modelManager.CreateModel(model, loggedInUserId);
                if (result == "Success")
                {
                    TempData["NotificationSuccess"] = result;
                    return RedirectToAction("ListModel");
                }
                else
                {
                    TempData["NotificationInfo"] = result;
                    return View();
                }
            }
            return View(model);
        }

        public ActionResult ListModel()
        {
            var models = _modelManager.GetModelList();
            return View(models);
        }

        [HttpGet]
        public ActionResult EditModel(int id)
        {
            var model = _modelManager.GetModel(id);
            ViewBag.BrandId = new SelectList(_modelManager.GetBrandsDropDown(), "Id", "Name");
            return View(model);
        }

        [HttpPost]
        public ActionResult EditModel(AdminModelViewModel viewModel)
        {
            int loggedInUserId = (int)Session["loggedInId"];
            var result = _modelManager.EditModel(viewModel, loggedInUserId);
            if (result == "Success")
            {
                TempData["NotificationSuccess"] = result;
                return RedirectToAction("ListModel");
            }
            else
            {
                TempData["NotificationInfo"] = result;
                return View();
            }
        }

        [HttpGet]
        public ActionResult DeleteModel(int id)
        {
            int loggedInUserId = (int)Session["loggedInId"];
            var result = _modelManager.DeleteModel(id, loggedInUserId);
            if (result == "Success")
            {
                TempData["NotificationSuccess"] = result;
                return RedirectToAction("ListModel");
            }
            else
            {
                TempData["NotificationInfo"] = result;
                return View();
            }
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

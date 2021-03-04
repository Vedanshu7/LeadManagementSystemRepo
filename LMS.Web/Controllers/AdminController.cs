using LMS.Common.Enums;
using LMS.Web.Attributes;
using LMS.Web.BAL.Interface;
using LMS.Web.BAL.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;

namespace LMS.Web.Controllers
{
    [Authenticate]
    [Authorization(RolesEnum.Admin)]
    public class AdminController : Controller
    {
        private readonly IBrandManager _brandManager;
        private readonly IModelManager _modelManager;
        private readonly IDealerManager _dealerManager;

        public AdminController(IModelManager modelManager, IBrandManager brandManager, IDealerManager dealerManager)
        {
            _modelManager = modelManager;
            _brandManager = brandManager;
            _dealerManager = dealerManager;
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

        [HttpGet]
        public ActionResult CreateDealer()
        {
            var data = _modelManager.GetBrandsDropDown();
            List<SelectListItem> brands = new List<SelectListItem>();
            foreach(var item in data)
            {
                SelectListItem select = new SelectListItem();
                select.Text = item.Name;
                select.Value = item.Id.ToString();
                brands.Add(select);
            }
            ViewBag.BrandId = brands;
            return View();
        }

        [HttpPost]
        public ActionResult CreateDealer(AdminDealerViewModel model)
        {
            if (ModelState.IsValid)
            {
                int loggedInUserId = (int)Session["loggedInId"];
                var result = _dealerManager.CreateDealer(model, loggedInUserId);
                if (result == "Success")
                {
                    TempData["NotificationSuccess"] = result;
                    return RedirectToAction("ListDealer");
                }
                else
                {
                    TempData["NotificationInfo"] = result;
                    return View();
                }
            }
            return View(model);
        }

        public ActionResult ListDealer()
        {
            var dealers = _dealerManager.GetDealers();
            return View(dealers);
        }

        [HttpGet]
        public ActionResult EditDealer(int id)
        {
            var dealer = _dealerManager.GetDealer(id);
            var data = _modelManager.GetBrandsDropDown();
            List<SelectListItem> brands = new List<SelectListItem>();
            foreach (var item in data)
            {
                SelectListItem select = new SelectListItem();
                select.Text = item.Name;
                select.Value = item.Id.ToString();
                if (dealer.Brands.Contains(item.Id))
                {
                    select.Selected = true;
                }
                brands.Add(select);
            }
            ViewBag.BrandId = brands;
            return View(dealer);
        }

        [HttpPost]
        public ActionResult EditDealer(AdminDealerViewModel viewModel)
        {
            int loggedInUserId = (int)Session["loggedInId"];
            var result = _dealerManager.EditDealer(viewModel, loggedInUserId);
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
    }
}

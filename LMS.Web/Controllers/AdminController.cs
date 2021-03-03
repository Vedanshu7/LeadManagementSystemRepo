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
        private readonly IModelManager _modelManager;
        
        public AdminController(IModelManager modelManager)
        {
            _modelManager = modelManager;
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
    }
}

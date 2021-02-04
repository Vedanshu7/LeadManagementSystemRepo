using LMS.Web.BAL.Interface;
using LMS.Web.BAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LMS.Web.Attributes;
using LMS.Common;

namespace LMS.Web.Controllers
{
    //TODO: Check if logged in User is Dealer
    [Authenticate]
    [Authorization(RolesEnum.Dealer)]
    public class DealerController : Controller
    {
        private readonly IUserManager _userManager;
        private readonly ILeadManager _leadManager;
        public DealerController(IUserManager userManager, ILeadManager leadManager)
        {
            _userManager = userManager;
            _leadManager = leadManager;
        }

        public ActionResult Index()
        {
            return Content("This is Dealer");
        }

        [HttpGet]
        public ActionResult CreateUser()
        {
            return View();
        }

        // POST: User
        [HttpPost]
        public ActionResult CreateUser(UserViewModel user)
        {
            if(ModelState.IsValid)
            {
                int dealerId = (int)Session["id"];
                var data = _userManager.CreateUser(user, dealerId);
                if (data == true)
                {
                    return Content("Success");
                }
                return Content("User Already Exist");
            }
            return View(user);
        }

        [HttpGet]
        public ActionResult Details()
        {
            return View(_userManager.UserDetail());
        }

        [HttpGet]
        public ActionResult EditUser(int Id)
        {
            UserViewModel user = _userManager.GetUser(Id);
            return View(user);
        }


        [HttpPost]
        public ActionResult EditUser(UserViewModel user)
        {
            int dealerId = (int)Session["id"];
            if (_userManager.EditUser(user, dealerId))
            {
                return Content("Success");
            }
            return Content("Failed!!");
        }

        [HttpGet]
        public ActionResult LeadList()
        {
            int dealerId = (int)Session["id"];
            List<DealerLeadViewModel> dealerLeadViewModels = _leadManager.GetDealerLeadList(dealerId);
            return View(dealerLeadViewModels);
        }

    }
}
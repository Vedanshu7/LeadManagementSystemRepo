using LMS.Web.BAL.Interface;
using LMS.Web.BAL.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;
using LMS.Web.Attributes;
using LMS.Common;
using System;

namespace LMS.Web.Controllers
{
    [Authenticate]
    [Authorization(RolesEnum.DealerManager)]
    public class DealerManagerController : Controller
    {
        private readonly IUserManager _userManager;
        private readonly ILeadManager _leadManager;
        public DealerManagerController(IUserManager userManager, ILeadManager leadManager)
        {
            _userManager = userManager;
            _leadManager = leadManager;
        }

        public ActionResult Index()
        {
            return Content("Dealer Dashboard");
        }

        [HttpGet]
        public ActionResult CreateUser()
        {
            ViewBag.RoleId = new SelectList(_userManager.GetUserRoleDropDown(), "Id", "Name");
            return View();
        }

        // POST: User
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateUser(UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                int dealerId = (int)Session["dealerId"];
                user.Password = PasswordEncryptor.Encryptor.Encryption(user.Password);
                var result = _userManager.CreateUser(user, dealerId);
                if (result == "Success")
                {
                    TempData["NotificationSuccess"] = result;
                    return RedirectToAction("UserList", "DealerManager");
                }
                else
                {
                    TempData["NotificationInfo"] = result;
                    return View();
                }
            }
            return View(user);
        }

        [HttpGet]
        public ActionResult UserList()
        {
            int dealerId = (int)Session["dealerId"];
            var users = _userManager.GetUsers(dealerId);
            return View(users);
        }

        [HttpGet]
        public ActionResult UserDetail(int Id)
        {
            int dealerId = (int)Session["dealerId"];
            UserViewModel user = _userManager.GetUser(dealerId, Id);
            return View(user);
        }

        [HttpGet]
        public ActionResult LeadDetail(int Id)
        {
            int dealerId = (int)Session["dealerId"];
            DealerLeadViewModel selectedLead = _leadManager.GetLeadDetailForDealer(Id, dealerId);
            return View(selectedLead);
        }

        [HttpGet]
        public ActionResult EditUser(int Id)
        {
            int dealerId = (int)Session["dealerId"];
            UserViewModel user = _userManager.GetUser(dealerId, Id);

            ViewBag.RoleId = new SelectList(_userManager.GetUserRoleDropDown(), "Id", "Name");
            return View(user);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditUser(UserViewModel user)
        {
            int dealerId = (int)Session["dealerId"];
            var result = _userManager.EditUser(user, dealerId);
            if (result == "Success")
            {
                TempData["NotificationSuccess"] = result;
                return RedirectToAction("UserList", "DealerManager");
            }
            else
            {
                TempData["NotificationInfo"] = result;
                return View();
            }
        }

        [HttpGet]
        public ActionResult LeadList()
        {
            int dealerId = (int)Session["dealerId"];
            int loggedInUserId = (int)Session["loggedInId"];
            List<DealerLeadViewModel> dealerLeadViewModels = _leadManager.GetDealerLeadList(dealerId);
            ViewBag.LeadTypeId = new SelectList(_leadManager.GetLeadTypeDropDown(), "Id", "DisplayName");
            ViewBag.LeadStatusId = new SelectList(_leadManager.GetLeadStatusDropDown(loggedInUserId), "Id", "DisplayName");

            var viewModel = new LeadViewModel() { Dealers = dealerLeadViewModels };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult LeadList(LeadViewModel viewModel)
        {
            var loggedInUserId = (int)Session["loggedInId"];
            //TODO: Check Start And End Date Is Valid Range
            var leadList = _leadManager.GetFilteredLeadList(viewModel.Filters, loggedInUserId);

            ViewBag.LeadTypeId = new SelectList(_leadManager.GetLeadTypeDropDown(), "Id", "DisplayName");
            ViewBag.LeadStatusId = new SelectList(_leadManager.GetLeadStatusDropDown(loggedInUserId), "Id", "DisplayName");

            viewModel.Dealers = leadList;

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult AssignLeadGet(int leadId) //Selected leadId
        {
            //Check Lead Type, return Selected Lead, Users based on LeadType
            int dealerId = (int)Session["dealerId"];
            DealerLeadViewModel selectedLead = _leadManager.GetLeadDetailForDealer(leadId, dealerId);
            if (selectedLead == null)
            {
                return RedirectToAction("LeadList");
            }
            List<UserViewModel> users = _userManager.GetUsersByLeadType(leadId);

            AssignLeadViewModel viewModel = new AssignLeadViewModel();
            viewModel.selectedLead = selectedLead;
            viewModel.users = users;

            return View("AssignLead", viewModel);
        }

        [HttpGet]
        public ActionResult AssignLeadConfirm(int selectedUserId, int leadId)
        {
            int dealerId = (int)Session["dealerId"];
            var result = _leadManager.AssignLeadForDealer(selectedUserId, leadId, dealerId);
            if (result == "Success")
            {
                TempData["NotificationSuccess"] = result;
                return RedirectToAction("LeadList");
            }
            else
            {
                TempData["NotificationInfo"] = result;
                return RedirectToAction("LeadList");
            }
        }

        [NonAction]
        public ActionResult DeAssignLead(int leadId)
        {
            int dealerId = (int)Session["dealerId"];
            var result = _leadManager.DeAssignLeadForDealer(leadId, dealerId);
            if (result == "Success")
            {
                TempData["NotificationSuccess"] = result;
                return RedirectToAction("LeadList");
            }
            else
            {
                TempData["NotificationInfo"] = result;
                return RedirectToAction("LeadList");
            }
        }

        [HttpGet]
        public ActionResult Modify(string operation, int leadId)
        {
            switch (operation)
            {
                case "Assign":
                    return RedirectToAction("AssignLeadGet", new { leadId = leadId });
                case "DeAssign":
                    return DeAssignLead(leadId);
                default:
                    return RedirectToAction("LeadList");
            }

        }
    }
}
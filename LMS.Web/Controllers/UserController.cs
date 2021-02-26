using System.Collections.Generic;
using System.Web.Mvc;
using LMS.Common;
using LMS.Web.Attributes;
using LMS.Web.BAL.Interface;
using LMS.Web.BAL.ViewModels;

namespace LMS.Web.Controllers
{
    [Authenticate]
    [Authorization(RolesEnum.Sales, RolesEnum.AfterSales)]
    public class UserController : Controller
    {
        private readonly ILeadManager _leadManager;
        public UserController(ILeadManager leadManager)
        {
            _leadManager = leadManager;
        }

        public ActionResult Index()
        {
            return RedirectToAction("UserDashboard");
        }
        [HttpGet]
        public ActionResult UserDashboard()
        {
            int loggedInUserId = (int)Session["loggedInId"];
            UserDashboardViewModel user = new UserDashboardViewModel();
            user.userAssignedLeadList = _leadManager.GetLeadListForUserDashboard(loggedInUserId);
            user.userNewLeadList = _leadManager.GetNewLeadListForUserDashboard(loggedInUserId);            
            return View(user);
        }

        [HttpGet]
        public ActionResult LeadList()
        {
            int loggedInUserId = (int)Session["loggedInId"];
            FilterLeadListViewModel filter = new FilterLeadListViewModel();
            List<DealerLeadViewModel> leads = _leadManager.GetLeadList(filter, loggedInUserId);
            var viewModel = new LeadViewModel() { Leads = leads, Filters = filter };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult LeadList(LeadViewModel viewModel)
        {
            int loggedInUserId = (int)Session["loggedInId"];
            var leadList = _leadManager.GetLeadList(viewModel.Filters, loggedInUserId);
            viewModel.Leads = leadList;
            if (viewModel.Filters.leadStatusId != null)
            {
                viewModel.Filters.flag = true;
            }
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult GetLeadStatusDropDown()
        {
            var loggedInUserId = (int)Session["loggedInId"];
            var leadstatusforuserlist = _leadManager.GetLeadStatusDropDown(loggedInUserId, "");
            return Json(leadstatusforuserlist, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult LeadDetails(int id)
        {
            int loggedInUserId = (int)Session["loggedInId"];
            UserLeadViewModel lead = _leadManager.GetLeadDetailForUser(loggedInUserId, id);
            if (lead != null)
            {
                //Populating LeadStatus DropDown
                ViewBag.LeadStatusId = new SelectList(_leadManager.GetLeadStatusDropDown(loggedInUserId, ""), "Id", "DisplayName");
                return View(lead);
            }
            else
            {
                TempData["NotificationInfo"] = "Invalid operation";
                return RedirectToAction("Index", "Sales");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult LeadDetails(UserLeadViewModel model)
        {
            int loggedInUserId = (int)Session["loggedInId"];
            string result = _leadManager.UpdateLeadDetails(model, loggedInUserId);
            if (result == "Success")
            {
                TempData["NotificationSuccess"] = result;
                return RedirectToAction("Index", "User");
            }
            else
            {
                TempData["NotificationInfo"] = result;
                return View();
            }
        }

        [HttpGet]
        public ActionResult AssignLead(int leadId)
        {
            int loggedInUserId = (int)Session["loggedInId"];
            var result = _leadManager.AssignLeadForUser(loggedInUserId, leadId);
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
        public ActionResult DeAssignLead(int leadId)
        {
            int loggedInUserId = (int)Session["loggedInId"];
            var result = _leadManager.DeAssignLeadForUser(loggedInUserId, leadId);
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
    }
}

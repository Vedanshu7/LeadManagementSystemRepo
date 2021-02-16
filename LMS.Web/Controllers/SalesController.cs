using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LMS.Common;
using LMS.Web.Attributes;
using LMS.Web.BAL.Interface;
using LMS.Web.BAL.ViewModels;

namespace LMS.Web.Controllers
{
    [Authenticate]
    [Authorization(RolesEnum.Sales)]
    public class SalesController : Controller
    {
        //TODO:Add lead assign & deassign.
        private readonly ILeadManager _leadManager;
        public SalesController(ILeadManager leadManager)
        {
            _leadManager = leadManager;
        }

        public ActionResult Index()
        {
            return RedirectToAction("LeadList");
        }

        public ActionResult LeadList()
        {
            int loggedInUserId = (int)Session["loggedInId"];
            List<UserLeadViewModel> list = _leadManager.GetUserLeadList(loggedInUserId);
            return View(list);
        }

        [HttpGet]
        public ActionResult LeadDetails(int id)
        {
            UserLeadViewModel lead = _leadManager.GetLeadDetailForUser(id);
            return View(lead);
        }

        [HttpPost]
        public ActionResult LeadDetails(UserLeadViewModel model)
        {
            //TODO:Dropdown change in lead details page

            int loggedInUserId = (int)Session["loggedInId"];
            bool result = _leadManager.UpdateLeadDetails(model, loggedInUserId);
            if (result)
                return RedirectToAction("LeadList", "Sales");
            return View(); //TODO: Notify Error Occurred
        }

        [HttpGet]
        public ActionResult AssignLead(int leadId)
        {
            int loggedInUserId = (int)Session["loggedInId"];
            var result = _leadManager.AssignLeadForUser(loggedInUserId, leadId);
            if (result)
            {
                return RedirectToAction("LeadList");
            }
            else
            {
                //TODO: Add Error Notification
                return RedirectToAction("LeadList");
            }
        }

        [HttpGet]
        public ActionResult DeAssignLead(int leadId)
        {
            var result = _leadManager.DeAssignLeadForUser(leadId);
            if (result)
            {
                return RedirectToAction("LeadList");
            }
            else
            {
                //TODO: Add Error Notification
                return RedirectToAction("LeadList");
            }
        }
    }
}

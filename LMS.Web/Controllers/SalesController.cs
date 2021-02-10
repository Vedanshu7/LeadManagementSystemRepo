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
        private readonly ISalesLeadManager _salesLeadManager;
        public SalesController(ISalesLeadManager salesleadManager)
        {
            _salesLeadManager = salesleadManager;
        }
        // GET: Sales
        public ActionResult Index()
        {
            int loggedInUserId = (int)Session["loggedInId"];
            List<SalesLeadViewModel> list = _salesLeadManager.GetSalesLeadList(loggedInUserId);
            return View(list);
        }

        [HttpGet]
        // GET: Sales/Details/5
        public ActionResult Details(int id)
        {
            SalesLeadViewModel list = _salesLeadManager.GetLeadDetail(id);
            return View(list);
        }

        [HttpPost]
        public ActionResult Details(SalesLeadViewModel model)
        {
            //TODO:Dropdown change in lead details page

            int loggedInUserId = (int)Session["loggedInId"];
            bool result = _salesLeadManager.UpdateLeadDetails(model, loggedInUserId);
            if (result)
                return RedirectToAction("Index", "Sales");
            return View(); //TODO: Notify Error Occurred
        }

        [HttpGet]
        public ActionResult AssignLead(int leadId)
        {
            int loggedInUserId = (int)Session["loggedInId"];
            var result = _salesLeadManager.AssignLead(loggedInUserId, leadId);
            if (result)
            {
                return RedirectToAction("Index");
            }
            else
            {
                //TODO: Add Error Notification
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public ActionResult DeAssignLead(int leadId)
        {
            var result = _salesLeadManager.DeAssignLead(leadId);
            if (result)
            {
                return RedirectToAction("Index");
            }
            else
            {
                //TODO: Add Error Notification
                return RedirectToAction("Index");
            }
        }
    }
}

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
    [Authorization(RolesEnum.AfterSales)]
    public class AfterSalesController : Controller
    {
        private readonly ISalesLeadManager _salesleadManager;
        public AfterSalesController(ISalesLeadManager salesleadManager)
        {
            _salesleadManager = salesleadManager;
        }
        // GET: AfterSales
        public ActionResult Index()
        {
            int dealerId = (int)Session["dealerId"];
            int loggedInUserId = (int)Session["loggedInId"];
            List<SalesLeadViewModel> list = _salesleadManager.GetSalesLeadList(dealerId, loggedInUserId);
            return View(list);
        }

        // GET: AfterSales/Details/5
        public ActionResult Details(int id)
        {
            SalesLeadViewModel list = _salesleadManager.GetLeadDetail(id);
            return View(list);
        }

        [HttpPost]
        public ActionResult Details(SalesLeadViewModel model)
        {
            int loggedInUserId = (int)Session["loggedInId"];
            bool result = _salesleadManager.UpdateLeadDetails(model, loggedInUserId);
            if (result)
                return RedirectToAction("Index", "Sales");
            return View();
        }
        // GET: AfterSales/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AfterSales/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AfterSales/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AfterSales/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AfterSales/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AfterSales/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LMS.Common;
using LMS.Web.Attributes;

namespace LMS.Web.Controllers
{
    [Authenticate]
    [Authorization(RolesEnum.AfterSales)]
    public class AfterSalesController : Controller
    {
        // GET: AfterSales
        public ActionResult Index()
        {
            return Content("This is AfterSales");
        }

        // GET: AfterSales/Details/5
        public ActionResult Details(int id)
        {
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

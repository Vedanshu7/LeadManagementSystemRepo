﻿using LMS.Web.BAL.Interface;
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
            return View();
        }

        // POST: User
        [HttpPost]
        public ActionResult CreateUser(UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                int dealerId = (int)Session["dealerId"];
                var data = _userManager.CreateUser(user, dealerId);
                if (data == true)
                {
                    TempData["NotificationSuccess"] = "User created successfully.";
                    return RedirectToAction("UserDetails","DealerManager");
                }
                else
                {
                      TempData["NotificationInfo"] = "User already register with this details.";
                      return View();
                }
            }
            return View(user);
        }

        [HttpGet]
        public ActionResult UserDetails()
        {
            int dealerId = (int)Session["dealerId"];
            var users = _userManager.UserDetails(dealerId);
            return View(users);
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
            int dealerId = (int)Session["dealerId"];
            if (_userManager.EditUser(user, dealerId))
            {
                TempData["NotificationSuccess"] = "User details update successfully.";
                return RedirectToAction("UserDetails","DealerManager");
            }
            else
            {
                TempData["NotificationInfo"] = "Error occure while update details.";
                return View();
            }
        }

        [HttpGet]
        public ActionResult LeadList()
        {
            int dealerId = (int)Session["dealerId"];
            List<DealerLeadViewModel> dealerLeadViewModels = _leadManager.GetDealerLeadList(dealerId);
            return View(dealerLeadViewModels);
        }

        [HttpGet]
        public ActionResult AssignLeadGet(int leadId) //Selected leadId
        {
            //Check Lead Type, return Selected Lead, Users based on LeadType

            DealerLeadViewModel selectedLead = _leadManager.GetLead(leadId);
            List<UserViewModel> users = _userManager.GetUsers(leadId);

            AssignLeadViewModel viewModel = new AssignLeadViewModel();
            viewModel.selectedLead = selectedLead;
            viewModel.users = users;

            return View("AssignLead", viewModel);
        }

        [HttpGet]
        public ActionResult AssignLeadConfirm(int selectedUserId, int leadId)
        {
            var result = _leadManager.AssignLead(selectedUserId,leadId);
            if (result)
            {
                TempData["NotificationSuccess"] = "Lead assign successfully to the user.";
                return RedirectToAction("LeadList");
            }
            else
            {
                //TODO: Add Error Notification
                TempData["NotificationInfo"] = "Error occure while assigning the user.";
                return RedirectToAction("LeadList");
            }
        }
        
        [NonAction]
        public ActionResult DeAssignLead(int leadId)
        {
            var result = _leadManager.DeAssignLead(leadId);
            if (result)
            {
                TempData["NotificationSuccess"] = "Lead De-Assign successful.";
                return RedirectToAction("LeadList");
            }
            else
            {
                //TODO: Add Error Notification
                TempData["NotificationInfo"] = "Error occure while De-Assigning from the user.";
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
using LMS.Web.BAL.Interface;
using LMS.Web.BAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserManager _userManager;
        public UserController(IUserManager userManager)
        {
            _userManager =userManager;
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
            var data = _userManager.CreateUser(user);
            if(data==true)
            {
                return Content("Success");
            }
            return Content("User Already Exist");
        }

        [HttpGet]
        public ActionResult Details()
        {
            return View(_userManager.UserDetail());
        }

        [HttpGet]
        public ActionResult EditUser(int Id)
        {
            //UserViewModel user = _userManager.UserDetail();
            //Use Automaper Here
            return View();
        }


        [HttpPost]
        public ActionResult EditUser(UserViewModel user)
        {
            return View();
        }

    }
}
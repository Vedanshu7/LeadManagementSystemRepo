using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LMS.Web.BAL.Interface;
using LMS.Web.BAL.ViewModels;

namespace LMS.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogin _loginManager;

        public LoginController(ILogin loginManager)
        {
            _loginManager = loginManager;
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public string Login(LoginViewModel loginViewObj)
        {
            var result = _loginManager.Login(loginViewObj);

            switch (result)
            {
                case 1:
                    return "Success";
                case 2:
                    return "Username of password don't match";
                case 3:
                    return "Incorrect password";
                default:
                    return "Error occurred";
            }
        }
    }
}
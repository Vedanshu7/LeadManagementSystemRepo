using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LMS.Web.BAL.Authentication;
using LMS.Web.BAL.Interface;
using LMS.Web.BAL.ViewModels;
using LMS.Web.BAL.Token;
using WebMatrix.WebData;
using System.Configuration;
using System.IO;

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


        public ActionResult EmailNotify()
        {
            return View();
        }


        // POST: Login
        [HttpPost]
        public string Index(LoginViewModel loginViewObj)
        {
            var result = _loginManager.Login(loginViewObj);

            switch (result)
            {
                case 1:
                    return "Welcome!!!";
                case 2:
                    return "Username of password don't match";
                case 3:
                    return "Incorrect password";
                default:
                    return "Error occurred";
            }
        }


        [HttpGet]
        public ActionResult ResetPassword(string Id, string code)
        {
            string userEmail = TokenManager.ValidateToken(code); //Returns the email which was used to Generate the token
            if (userEmail != Id)
            {
                return View("Login");
            }
            else
            {
                Session["token"] = code;
                Session["IsEmailSent"] = null;
                return View("ResetPassword");
            }
        }

        [HttpPut]
        public ActionResult ResetPassword(ResetPasswordViewModel resetPassword) //TODO: Use ViewModel
        {
            if (ModelState.IsValid)
            {
                if (Session["token"] != null)
                {
                    string token = Session["token"].ToString();
                    string userEmail = TokenManager.ValidateToken(token);

                    int result = _loginManager.ResetPassword(userEmail, resetPassword);
                    Session["token"] = null;
                    switch (result)
                    {
                        case 0:
                            return Content("Error occured"); //TODO: Return Error Page
                        case 1: //Success
                            return View("Login");
                        default:
                            return Content("Unexpected error occured");
                    }
                }
                else
                {
                    return View("ForgotPassword");
                }
            }
            else
            {
                return View("ResetPassword");
            }
        }

        // GET : ForgotPassword
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View("ForgotPassword");
        }

        [HttpPost]
        public ActionResult ForgotPassword(string userEmail)
        {
            //TODO: Check if email exist

            string token = TokenManager.GenerateToken(userEmail);
            if (token == null)
            {
                return View("Login"); //Invalid Token
            }

            //Form Email
            string mailText = FormEmail(userEmail, token);

            //Get and set the AppSettings using configuration manager.
            EmailManager.AppSettings(out var userId, out var password, out var smtpPort, out var host);

            //Call send email methods.
            EmailManager.SendEmail(userId, "Reset Your Password", mailText, userEmail, userId, password, smtpPort, host);

            Session["IsEmailSent"] = true;

            return View("Login"); //TODO: Mail Has Been Sent View/Notification
        }

        [NonAction]
        private string FormEmail(string userEmail, string token)
        {
            //Set the Email Template
            string filePath = Server.MapPath("~/EmailTemplate/") + "ResetPassword.html";
            StreamReader str = new StreamReader(filePath);
            string mailText = str.ReadToEnd();
            str.Close();

            //Replace [newusername] = signup user name 
            mailText = mailText.Replace("[Product Name]", "Lead Management System");

            mailText = mailText.Replace("{{name}}", userEmail);

            var lnkHref = "https://localhost:44381/Authentication/ResetPassword/" + "?Id=" + userEmail + "&code=" + token;

            mailText = mailText.Replace("{{action_url}}", lnkHref);

            mailText = mailText.Replace("[Company Name, LLC]", "Lead Management System LLC");

            return mailText;
        }
    }
}
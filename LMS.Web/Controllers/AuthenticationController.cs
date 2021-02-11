using System.Web.Mvc;
using LMS.Web.BAL.Authentication;
using LMS.Web.BAL.Interface;
using LMS.Web.BAL.ViewModels;
using LMS.Web.BAL.Token;
using System.IO;
using LMS.Common;

namespace LMS.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ILoginManager _loginManager;

        public AuthenticationController(ILoginManager loginManager)
        {
            _loginManager = loginManager;
        }

        [HttpGet]
        public ActionResult Login()
        {
            //Check if already logged in
            if (Session["loggedInId"] != null)
            {
                var currentRole = (RolesEnum)Session["role"];
                switch (currentRole)
                {
                    case RolesEnum.DealerManager:
                        return RedirectToAction("Index", "DealerManager");
                    case RolesEnum.Sales:
                        return RedirectToAction("Index", "Sales");
                    case RolesEnum.AfterSales:
                        return RedirectToAction("Index", "AfterSales");
                }
            }
            return View(); //TODO: Redirect to page
        }

        // POST: Login
        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewObj)
        {
            if (ModelState.IsValid)
            {
                loginViewObj.Email = loginViewObj.Email.ToLower();

                //Log in the user
                var loginResult = _loginManager.Login(loginViewObj);

                //Check result
                switch (loginResult.result)
                {
                    case LoginResultEnum.Success:
                        Session["loggedInId"] = loginResult.LoggedInUserId;
                        Session["role"] = loginResult.role;
                        switch (loginResult.role)
                        {
                            case RolesEnum.DealerManager:
                                Session["dealerId"] = loginResult.DealerId;
                                return RedirectToAction("Index", "DealerManager");
                            case RolesEnum.Sales:
                                Session["loggedInId"] =loginResult.LoggedInUserId;
                                return RedirectToAction("Index", "Sales");
                            case RolesEnum.AfterSales:
                                Session["loggedInId"] = loginResult.LoggedInUserId;
                                return RedirectToAction("Index", "AfterSales");
                        }
                        break;

                    case LoginResultEnum.Invalid:
                        ModelState.AddModelError("Password", "Username or password don't match");
                        return View(loginViewObj);

                    case LoginResultEnum.NotFound:
                        ModelState.AddModelError("Email", "User does not exist");
                        return View(loginViewObj);
                }
            }
            return View(loginViewObj);
        }

        [HttpGet]
        public ActionResult ResetPassword(string Id, string code)
        {
            if (string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(code))
                return View("Login");

            string userEmail = TokenManager.ValidateToken(code); //Returns the email which was used to Generate the token
            if (userEmail.Equals(Id))
            {
                return View("Login");
            }
            Session["token"] = code;
            Session["IsEmailSent"] = null;
            return View("ResetPassword");
        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordViewModel resetPassword)
        {
            if (ModelState.IsValid)
            {
                if (Session["token"] != null)
                {
                    string token = Session["token"].ToString();
                    string userEmail = TokenManager.ValidateToken(token);

                    string result = _loginManager.ResetPassword(userEmail, resetPassword);
                    Session["token"] = null;

                    return Content(result); //TODO: Pass this as message to view
                }
                return View("ForgotPassword");

            }
            return View("ResetPassword");
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
            if (!string.IsNullOrEmpty(userEmail))
            {
                //Check if user exists or not
                if (_loginManager.IsValidUser(userEmail))
                {
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
                else
                {
                    return Content("User not found"); //TODO: Pass error to view
                }
            }
            return View();
        }

        public ActionResult Unauthorized()
        {
            return Content("Unauthorized"); //TODO: Create Unauthorized Page
        }

        public ActionResult LogOff()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login", "Authentication");
        }

        [NonAction]
        private string FormEmail(string userEmail, string token)
        {
            //Set the Email Template
            string filePath = Server.MapPath("~/Views/EmailTemplate/") + "ResetPassword.html";
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
using System.Web.Mvc;
using LMS.Common.Email;
using LMS.Web.BAL.Interface;
using LMS.Web.BAL.ViewModels;
using LMS.Web.BAL.Token;
using System.IO;
using LMS.Common;
using LMS.Common.Enums;
using LMS.Web.Attributes;

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
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Error()
        {
            return View();
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
                    case RolesEnum.AfterSales:
                        return RedirectToAction("Index", "User");
                    case RolesEnum.Admin:
                        return RedirectToAction("Index", "Admin");
                    default:
                        return RedirectToAction("Error");
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewObj)
        {
            if (ModelState.IsValid)
            {
                loginViewObj.Email = loginViewObj.Email.ToLower();
                loginViewObj.Password = PasswordEncryptor.Encryptor.Encryption(loginViewObj.Password);
                //Log in the user
                var loginResult = _loginManager.Login(loginViewObj);
                string path = string.Empty;
                if (Session["RedirectUrl"] != null)
                {
                    path = Session["RedirectUrl"].ToString();
                    Session["RedirectUrl"] = null;
                }
                //Check result
                switch (loginResult.result)
                {
                    case LoginResultEnum.Success:
                        Session["loggedInId"] = loginResult.LoggedInUserId;
                        Session["role"] = loginResult.role;
                        Session["username"] = loginResult.UserName;
                        switch (loginResult.role)
                        {
                            case RolesEnum.DealerManager:
                                Session["dealerId"] = loginResult.DealerId;
                                if (!path.Equals(string.Empty))
                                    return Redirect(path);
                                return RedirectToAction("Index", "DealerManager");
                            case RolesEnum.Sales:
                            case RolesEnum.AfterSales:
                                Session["loggedInId"] = loginResult.LoggedInUserId;
                                if (!path.Equals(string.Empty))
                                    return Redirect(path);
                                return RedirectToAction("Index", "User");
                            case RolesEnum.Admin:
                                Session["loggedInId"] = loginResult.LoggedInUserId;
                                if (!path.Equals(string.Empty))
                                    return Redirect(path);
                                return RedirectToAction("Index", "Admin");
                            default:
                                return RedirectToAction("Error");
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
            if (!userEmail.Equals(Id))
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
                    resetPassword.Password = PasswordEncryptor.Encryptor.Encryption(resetPassword.Password);
                    string result = _loginManager.ResetPassword(userEmail, resetPassword);
                    Session["token"] = null;

                    if (result == "Success")
                    {
                        TempData["NotificationSuccess"] = result;
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        TempData["NotificationInfo"] = result;
                        return View();
                    } 
                }
                return View("ForgotPassword");

            }
            return View("ResetPassword");
        }

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

                    TempData["NotificationSuccess"] = "Password Reset link has been sent to your mail id.";
                    return View("Login");
                }
                else
                {
                    TempData["NotificationInfo"] = "User not found.";
                    return View(); 
                }
            }
            return View();
        }

        [Authenticate]
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authenticate]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel change)
        {
            if (ModelState.IsValid)
            {
                int loggedInUserId = (int)Session["loggedInId"];
                change.CurrentPassword = PasswordEncryptor.Encryptor.Encryption(change.CurrentPassword);
                change.NewPassword = PasswordEncryptor.Encryptor.Encryption(change.NewPassword);
                var result = _loginManager.ChangePassword(change, loggedInUserId);
                if (result == "Success")
                {
                    TempData["NotificationSuccess"] = "Password Changed Successfully";
                    return RedirectToAction("Login");
                }
                else
                {
                    TempData["NotificationInfo"] = result;
                    return RedirectToAction("ChangePassword");
                }
            }
            else
            {
                return View(change);
            }
            
        }
        public ActionResult Unauthorized()
        {
            return View();
        }
        public ActionResult LogOff()
        {
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

            mailText = mailText.Replace("[Product Name]", "Lead Management System");
            mailText = mailText.Replace("{{name}}", userEmail);
            var lnkHref = "https://localhost:44381/Authentication/ResetPassword/" + "?Id=" + userEmail + "&code=" + token;
            mailText = mailText.Replace("{{action_url}}", lnkHref);
            mailText = mailText.Replace("[Company Name, LLC]", "Lead Management System LLC");
            return mailText;
        }
    }
}
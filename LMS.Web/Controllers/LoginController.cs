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
       
        public ActionResult ResetPassword(string Id, string code,string Password="",string ConfirmPassword="")
        {

            if (Password == "")
            {
                string useremail = TokenManager.ValidateToken(code);
                if (useremail != Id)
                {
                    return View("Login");
                }
                else
                {
                    Session["token"] = code;
                    return View("ResetPassword");
                }
            }
            else if (Password == ConfirmPassword)
            {
                if (Session["token"] != null)
                {
                    string token = Session["token"].ToString();
                    string user = TokenManager.ValidateToken(token);
                    LoginViewModel loginViewModel = new LoginViewModel();
                    loginViewModel.Email = user;
                    loginViewModel.Password = Password;
                    loginViewModel.Role = 1;
                    int i = _loginManager.RestPassword(loginViewModel);
                    Session["token"] = null;
                    switch (i)
                    {
                        case 1:
                            return View("Login");
                        default:
                            return Content(i.ToString());
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



        public ActionResult ForgotPassword(string Email)
        {
            if (Email!= null)
            {
                string To = Email, UserID, Password, SMTPPort, Host;
                string token = TokenManager.GenerateToken(Email);
                if (token == null)
                {
                    return View("Login");
                }
                else
                {
                    //Session["token"] = token;
                    //Create URL with above token
                    string FilePath = Server.MapPath("~/EmailTemplate/")+"ResetPassword.html";
                    StreamReader str = new StreamReader(FilePath);
                    string MailText = str.ReadToEnd();
                    str.Close();

                    //Repalce [newusername] = signup user name   
                    MailText = MailText.Replace("[Product Name]", "Lead Management System");

                    MailText = MailText.Replace("{{name}}", Email);

                    MailText = MailText.Replace("{{name}}", Email);

                    var lnkHref =  "https://localhost:44381/Login/ResetPassword/"+"?Id="+Email+"&code="+token;

                    MailText = MailText.Replace("{{action_url}}", lnkHref);

                    MailText = MailText.Replace("[Company Name, LLC]", "Lead Management System LLC");

                   
                    //HTML Template for Send email
                    string subject = "Reset Your Password";
                  
                    //Get and set the AppSettings using configuration manager.
                    EmailManager.AppSettings(out UserID, out Password, out SMTPPort, out Host);
                    //Call send email methods.
                    EmailManager.SendEmail(UserID, subject, MailText, To, UserID, Password, SMTPPort, Host);
                    return View("Login");
                }
            }
            else{
                return View("ForgotPassword");
            }
            
        }
    }
}
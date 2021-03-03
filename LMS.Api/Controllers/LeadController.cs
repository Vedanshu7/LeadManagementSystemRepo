using LMS.Common.Dtos;
using LMS.Api.BAL.Interface;
using System.Web.Http;
using System;
using System.IO;
using System.Web.Hosting;
using LMS.Common.Email;
using System.Configuration;
using LMS.Api.Attributes;
using LMS.Common.Enums;

namespace LMS.Api.Controllers
{
    [Authenticate]
    public class LeadController : ApiController
    {
        private readonly ILeadManager _leadManager;
        public LeadController(ILeadManager leadManager)
        {
            _leadManager = leadManager;
        }

        // POST: api/Lead/
        [HttpPost]
        public IHttpActionResult PostLead(LeadDto lead)
        {
            try
            {
                var result = _leadManager.AddLead(lead);

                switch (result)
                {
                    case LeadResultEnum.Success:
                        return Ok("Lead added");
                    case LeadResultEnum.Invalid:
                        return BadRequest("Brand does not exist for dealer");
                    case LeadResultEnum.ErrorOccurred:
                    default:
                        return InternalServerError();
                }
            }
            catch (Exception e)
            {
                SendAdminEmail(lead, e);
                //TODO: Add Logger
                return InternalServerError();
            }
        }

        [NonAction]
        private void SendAdminEmail(LeadDto lead, Exception e)
        {
            string filePath = HostingEnvironment.MapPath("~/Views/EmailTemplate/Email.html");
            StreamReader str = new StreamReader(filePath);
            string mailText = str.ReadToEnd();
            str.Close();
            string leadtable = _leadManager.GenerateLeadTable(lead);
            mailText = mailText.Replace("[Type]", "Admin");
            mailText = mailText.Replace("[Lead Details]", leadtable);
            mailText = mailText.Replace("[Exception]", e.Message + "<p style='color:red'><b>Failed To Add Lead</b></p>");
            EmailManager.AppSettings(out var userId, out var password, out var smtpPort, out var host);
            string adminMail = ConfigurationManager.AppSettings.Get("AdminID");
            //Call send email methods.
            EmailManager.SendEmail(userId, "Error Occurred", mailText, adminMail, userId, password, smtpPort, host);
        }
    }
}

﻿using LMS.Common.Dtos;
using LMS.Api.BAL.Interface;
using System.Web.Http;
using System;
using System.IO;
using System.Web.Hosting;
using LMS.Common.Email;
using System.Configuration;
using LMS.Api.Attributes;
using LMS.Common.Enums;
using System.Web.Http.Cors;
using log4net;

namespace LMS.Api.Controllers
{
    [Authenticate]
    public class LeadController : ApiController
    {
        private readonly ILeadManager _leadManager;
        private static readonly ILog Log = LogManager.GetLogger(typeof(LeadController));
        public LeadController(ILeadManager leadManager)
        {
            _leadManager = leadManager;
        }

        // POST: api/Lead/
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        public IHttpActionResult PostLead([FromBody]LeadDto lead)
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
                //Log.Error(e.Message, e);
                SendAdminEmail(lead, e);
                return InternalServerError();
            }
        }

        [NonAction]
        private void SendAdminEmail(LeadDto lead, Exception e)
        {
            string filePath = HostingEnvironment.MapPath("~/App_Data/EmailTemplate/Email.html");
            StreamReader str = new StreamReader(filePath);
            string mailText = str.ReadToEnd();
            str.Close();
            string leadtable = _leadManager.GenerateLeadTable(lead);
            mailText = mailText.Replace("[FirstName]", "Admin");
            mailText = mailText.Replace("[LastName]", "");
            mailText = mailText.Replace("[Message]", leadtable);
            mailText = mailText.Replace("[Exception]", e.Message + "<p style='color:red'><b>Failed To Add Lead</b></p>");
            EmailManager.AppSettings(out var userId, out var password, out var smtpPort, out var host);
            string adminMail = ConfigurationManager.AppSettings.Get("AdminID");
            //Call send email methods.
            EmailManager.SendEmail(userId, "Error Occurred", mailText, adminMail, userId, password, smtpPort, host);
        }
    }
}

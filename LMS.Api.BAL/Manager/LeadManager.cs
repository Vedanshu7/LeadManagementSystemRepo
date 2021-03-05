using LMS.Common.Dtos;
using LMS.Api.BAL.Interface;
using LMS.Api.DAL.Interface;
using LMS.Common;
using LMS.Common.Email;
using System.IO;
using System.Web;
using System.Web.Hosting;
using System.Collections.Generic;
using LMS.Common.Enums;

namespace LMS.Api.BAL.Manager
{
    public class LeadManager : ILeadManager
    {
        private readonly ILeadRepository _leadRepository;
        public LeadManager(ILeadRepository leadRepository)
        {
            _leadRepository = leadRepository;
        }

        public LeadResultEnum AddLead(LeadDto lead)
        {
            var result = _leadRepository.AddLead(lead);

            if (result.result == LeadResultEnum.Success)
            {
                //Form Email
                string mailText = FormEmail(lead.LeadTypeCode);

                //Get and set the AppSettings using configuration manager.
                EmailManager.AppSettings(out var userId, out var password, out var smtpPort, out var host);

                //Forming Mailing List as CSVs
                //TODO: Uncomment this line for real mailing list
                //CreateMailingList(result.mailingList);
                string testMailingList = "mihirj.joshi@thegatewaycorp.co.in";

                //Call send email methods.
                EmailManager.SendEmail(userId, "LMS", mailText, testMailingList, userId, password, smtpPort, host);

                return result.result;
            }
            return result.result;
        }
        public string GenerateLeadTable(LeadDto lead)
        {
            string table = "<ul>" +
                "<li> Customer Name: " + lead.CustomerName + "</li>" + "<li> Dealer Code: " + lead.DealerCode + "</li>" +
                "<li> Model Code: " + lead.ModelCode + "</li>" +
                "<li> Customer Email: " + lead.CustomerEmail + "</li>" +
                "<li> Contact Number: " + lead.CustomerContactNumber + "</li>" +
                "<li> LeadType Code: " + lead.LeadTypeCode + "</li>" + "<li> ServiceType: " + lead.ServiceType + "</li>" + "<li> Comments: " + lead.Comments + "</li>" +
                "</ul>";
            return table;
        }

        private string FormEmail(string Leadtype)
        {
            var User = "AfterSale";
            var link = "<button style='background - color:#333435;color:black;'" +
                " onclick='return window.location.href = 'https://localhost:44381/User/''>Check Your Leads</button>";
            if (Leadtype.Equals(Common.Constants.LeadType.Sales))
            {
                User = "Sales";
            }
            //Set the Email Template
            string filePath = HostingEnvironment.MapPath("~/App_Data/EmailTemplate/Email.html");
            StreamReader str = new StreamReader(filePath);
            string mailText = str.ReadToEnd();
            str.Close();
            mailText = mailText.Replace("[FirstName]",User);
            mailText = mailText.Replace("[LastName]","User");
            mailText = mailText.Replace("[Message]", "New Lead ");
            mailText = mailText.Replace("[Exception]", link);
            return mailText;
        }
        private string CreateMailingList(List<string> mailIds)
        {
            string sendingMailList = "";
            foreach (var item in mailIds)
            {
                if (sendingMailList == "")
                {
                    sendingMailList = item;
                }
                else
                {
                    sendingMailList = sendingMailList + "," + item;
                }
            }
            return sendingMailList;
        }
    }
}

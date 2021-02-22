using LMS.Common.Dtos;
using LMS.Api.BAL.Interface;
using LMS.Api.DAL.Interface;
using LMS.Common.Enums;
using LMS.Common.Email;

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
                //TODO: Pass mailing list
                string mailText = FormEmail(result.mailingList[0]);

                //Get and set the AppSettings using configuration manager.
                EmailManager.AppSettings(out var userId, out var password, out var smtpPort, out var host);

                //Call send email methods.
                //TODO: Pass mailing list
                EmailManager.SendEmail(userId, "New Lead Added", mailText, result.mailingList[0], userId, password, smtpPort, host);

                return result.result;
            }
            return result.result;
        }

        private string FormEmail(string userEmail)
        {
            string mailText = $"Hello, {userEmail}. Lead has been successfully added.";
            return mailText;
        }
    }
}

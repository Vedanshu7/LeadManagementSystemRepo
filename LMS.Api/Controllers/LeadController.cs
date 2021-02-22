using LMS.Common.Dtos;
using LMS.Api.BAL.Interface;
using System.Web.Http;
using System;

namespace LMS.Api.Controllers
{
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

                if (result == Common.Enums.LeadResultEnum.Success)
                {
                    return Ok("Lead added");
                }

                //TODO: Return error results
                return InternalServerError();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //TODO: Send Email
                return InternalServerError();
            }
        }

    }
}

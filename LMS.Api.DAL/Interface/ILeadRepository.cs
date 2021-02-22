using LMS.Common.Dtos;
using LMS.Common.Models;

namespace LMS.Api.DAL.Interface
{
    public interface ILeadRepository
    {
        LeadResult AddLead(LeadDto lead);
    }
}

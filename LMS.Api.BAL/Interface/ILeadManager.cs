using LMS.Common.Dtos;
using LMS.Common.Enums;

namespace LMS.Api.BAL.Interface
{
    public interface ILeadManager
    {
        LeadResultEnum AddLead(LeadDto lead);
        string GenerateLeadTable(LeadDto lead);
    }
}

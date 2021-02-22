using LMS.Common.Dtos;
using LMS.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Api.BAL.Interface
{
    public interface ILeadManager
    {
        LeadResultEnum AddLead(LeadDto lead);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Web.DAL.Enums
{
    public enum LoginResult : int
    {
        Success = 1,
        Invalid = 2,
        NotFound = 3
    }
}

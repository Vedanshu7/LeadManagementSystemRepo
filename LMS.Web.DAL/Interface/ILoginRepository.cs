using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Web.DAL.Interface
{
    public interface ILoginRepository
    {
        int Login(string email, string password, int role);

        int ResetPassword(string email, string password);
    }
}

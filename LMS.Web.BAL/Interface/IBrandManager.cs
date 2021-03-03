using LMS.Web.BAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Web.BAL.Interface
{
    public interface IBrandManager
    {
        string CreateBrand(AdminBrandViewModel model, int loggedInUserId);
        List<AdminBrandViewModel> GetBrandList();
        string EditBrand(AdminBrandViewModel model, int loggedInUserId);
        AdminBrandViewModel GetBrand(int id,int loggedInUserId);
    }
}

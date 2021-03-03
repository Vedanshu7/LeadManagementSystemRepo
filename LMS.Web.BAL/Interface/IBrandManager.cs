using LMS.Web.BAL.ViewModels;
using System.Collections.Generic;


namespace LMS.Web.BAL.Interface
{
    public interface IBrandManager
    {
        string CreateBrand(AdminBrandViewModel model, int loggedInUserId);
        List<AdminBrandViewModel> GetBrandList();
        string EditBrand(AdminBrandViewModel model, int loggedInUserId);
        AdminBrandViewModel GetBrand(int id);
    }
}

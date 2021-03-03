using LMS.Web.BAL.ViewModels;
using System.Collections.Generic;

namespace LMS.Web.BAL.Interface
{
    public interface IModelManager
    {
        string CreateModel(AdminModelViewModel model, int loggedInUserId);
        List<AdminModelViewModel> GetModelList();
        AdminModelViewModel GetModel(int id);
        IEnumerable<BrandViewModel> GetBrandsDropDown();
        string EditModel(AdminModelViewModel viewModel, int loggedInId);
        string DeleteModel(int id, int loggedInId);
    }
}

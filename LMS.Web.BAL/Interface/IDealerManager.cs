
using LMS.Web.BAL.ViewModels;
using System.Collections.Generic;

namespace LMS.Web.BAL.Interface
{
    public interface IDealerManager
    {
        string CreateDealer(AdminDealerViewModel viewModel, int loggedInUserId);
        AdminDealerViewModel GetDealer(int id);
        List<AdminDealerViewModel> GetDealers();
        string EditDealer(AdminDealerViewModel viewModel, int loggedInUserId);
    }
}

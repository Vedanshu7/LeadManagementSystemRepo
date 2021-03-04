using LMS.Web.DAL.Database;
using LMS.Web.DAL.Models;
using System.Collections.Generic;

namespace LMS.Web.DAL.Interface
{
    public interface IDealerRepository
    {
        string CreateDealer(Dealers dealer, int loggedInUserId, List<int> brands);
        List<DealerModel> GetDealers();
        Dealers GetDealer(int id);
        string EditDealer(Dealers dealer, int loggedInUserId, List<int> brands);
    }
}

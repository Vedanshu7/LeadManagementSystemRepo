
using LMS.Web.DAL.Database;
using LMS.Web.DAL.Models;
using System.Collections.Generic;

namespace LMS.Web.DAL.Interface
{
    public interface IModelRepository
    {
        string CreateModel(Database.Models model, int loggedInUserId);
        List<VehicleModel> GetModels();
        IEnumerable<Brands> GetBrandsDropDown();
        Database.Models GetModel(int id);
        string EditModel(Database.Models model, int loggedInUserId);
        string DeleteModel(int id, int loggedInUserId);
    }
}

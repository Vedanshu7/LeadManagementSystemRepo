using LMS.Web.DAL.Database;
using LMS.Web.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Web.DAL.Interface
{
    public interface IBrandRepository
    {
        string CreateBrand(Brands model);
        List<VehicleBrand> GetBrandList();
        string EditBrand(Brands model);
        Brands GetBrand(int id, int loggedInUserId);
    }
}

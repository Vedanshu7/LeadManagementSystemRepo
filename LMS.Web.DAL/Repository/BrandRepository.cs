using LMS.Web.DAL.Database;
using LMS.Web.DAL.Interface;
using LMS.Web.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace LMS.Web.DAL.Repository
{
    public class BrandRepository : IBrandRepository
    {
        private readonly LMSAzureEntities _db;
        public BrandRepository()
        {
            _db = new LMSAzureEntities();
        }

        public string CreateBrand(Brands model)
        {
            try
            {
                var result = _db.Brands.Any(m => m.BrandCode == model.BrandCode && m.IsActive == true);
                if (!result)
                {
                    model.CreatedDate = DateTime.Now;
                    model.IsActive = true;
                    _db.Brands.Add(model);
                    _db.SaveChanges();
                    return "Success";
                }
                else
                {
                    return "Error occured";
                }
            }
            catch (Exception e)
            {
                //TODO: Add logger.
                return "Error occured.";
            }
        }

        public List<VehicleBrand> GetBrandList()
        {

            var brands = _db.Brands.Join(_db.Users,
                b => b.CreatedBy,
                u => u.Id,
                (brand, user) => new { brand, user })
                .Select(bu => bu);
            List<VehicleBrand> vehicleBrands = new List<VehicleBrand>();
            foreach (var item in brands)
            {
                var brand = new VehicleBrand();
                brand.Id = item.brand.Id;
                brand.Name = item.brand.Name;
                brand.Brandcode = item.brand.BrandCode;
                brand.CreatedDate = item.brand.CreatedDate;
                brand.UpdatedDate = item.brand.UpdatedDate;
                brand.BrandUpdatedBy = item.user.Name;
                brand.BrandCreatedBy = item.user.Name;
                brand.IsActive = item.brand.IsActive;
                vehicleBrands.Add(brand);
            }
            return vehicleBrands;

        }

        public string EditBrand(Brands model)
        {
            try
            {
                var brandFromDb = _db.Brands.Where(m => m.Id == model.Id && m.IsActive == true).FirstOrDefault();
                if (brandFromDb != null)
                {
                    brandFromDb.Name = model.Name;
                    brandFromDb.BrandCode = model.BrandCode;          
                    brandFromDb.UpdatedDate = DateTime.Now;
                    brandFromDb.IsActive = model.IsActive;
                    _db.Entry(brandFromDb).State = EntityState.Modified;
                    _db.SaveChanges();
                    return "Success";
                }


                else
                {
                    return "Error occured.";
                }
            }
            catch (Exception)
            {
                return "Error occured.";
                throw;
            }
        }

        public Brands GetBrand(int id, int loggedInUserId)
        {
            var data = _db.Brands.Where(m => m.Id == id && m.CreatedBy == loggedInUserId).FirstOrDefault();
            return data;

        }
    }

}
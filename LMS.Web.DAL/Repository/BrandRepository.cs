using LMS.Web.DAL.Database;
using LMS.Web.DAL.Interface;
using LMS.Web.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
                .Select(bu => bu).ToList();

            var updateBrands = _db.Brands.Join(_db.Users,
                b => b.UpdatedBy,
                u => u.Id,
                (brand, user) => new { brand, user })
                .Select(bu => bu).ToList();
            List<VehicleBrand> vehicleBrands = new List<VehicleBrand>();
            for(int i=0;i< brands.Count;i++)
            {
                var brand = new VehicleBrand();
                brand.Id = brands[i].brand.Id;
                brand.Name = brands[i].brand.Name;
                brand.Brandcode = brands[i].brand.BrandCode;
                brand.CreatedDate = brands[i].brand.CreatedDate;
                brand.UpdatedDate = brands[i].brand.UpdatedDate;
                brand.BrandCreatedBy = brands[i].user.Name;
               
                brand.IsActive = brands[i].brand.IsActive;
                vehicleBrands.Add(brand);
            }
            foreach(var item in updateBrands)
            {
                var brand = vehicleBrands.Where(v => v.Id == item.brand.Id).First();
                brand.BrandUpdatedBy = item.user.Name;

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
                    brandFromDb.UpdatedBy = model.UpdatedBy;
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

        public Brands GetBrand(int id)
        {
            var data = _db.Brands.Where(m => m.Id == id).FirstOrDefault();
            return data;
        }
    }
}
using LMS.Web.DAL.Database;
using LMS.Web.DAL.Interface;
using LMS.Web.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace LMS.Web.DAL.Repository
{
    public class ModelRepository : IModelRepository
    {
        private readonly LMSAzureEntities _db;
        public ModelRepository()
        {
            _db = new LMSAzureEntities();
        }
        public string CreateModel(Database.Models model, int loggedInUserId)
        {
            try
            {
                var doesModelExist = _db.Models.Any(m => m.ModelCode == model.ModelCode);
                if (!doesModelExist)
                {
                    model.CreatedDate = DateTime.Now;
                    model.IsActive = model.IsActive;
                    model.IsDeleted = false;
                    _db.Models.Add(model);
                    _db.SaveChanges();
                    return "Success";
                }
                return "Model already exists.";
            }
            catch (Exception)
            {
                //TODO: Add Logger
                throw;
            }
        }

        public List<VehicleModel> GetModels()
        {
            var models = _db.Models.Join(_db.Users,
                model => model.CreatedBy,
                user => user.Id,
                (model, user) => new { model, user }).Where(mu => mu.model.IsDeleted == false)
                .Select(mu => mu).ToList();

            var updatedModels = _db.Models.Join(_db.Users,
                model => model.UpdatedBy,
                user => user.Id,
                (model, user) => new { model, user }).Where(mu => mu.model.IsDeleted == false)
                .Select(mu => mu).ToList();

            List<VehicleModel> vehicleModels = new List<VehicleModel>();
            for (int i = 0; i < models.Count; i++)
            {
                var vehicleModel = new VehicleModel();
                vehicleModel.Id = models[i].model.Id;
                vehicleModel.Name = models[i].model.Name;
                vehicleModel.Brand = models[i].model.Brands.Name;
                vehicleModel.FuelType = models[i].model.FuelType;
                vehicleModel.TransmissionType = models[i].model.TransmissionType;
                vehicleModel.ExteriorColor = models[i].model.ExteriorColor;
                vehicleModel.InteriorColor = models[i].model.InteriorColor;
                vehicleModel.ModelCode = models[i].model.ModelCode;
                vehicleModel.CreatedDate = models[i].model.CreatedDate;
                vehicleModel.UpdatedDate = models[i].model.UpdatedDate;
                vehicleModel.CreatedBy = models[i].user.Name;
                //if (updatedModels.Contains(models[i]))
                //{
                //    var key = updatedModels.IndexOf(models[i]);
                //    vehicleModel.UpdatedBy = updatedModels[key].user.Name;
                //}
                vehicleModel.IsActive = models[i].model.IsActive;
                vehicleModels.Add(vehicleModel);
            }

            foreach (var item in updatedModels)
            {
                var model = vehicleModels.Where(v => v.Id == item.model.Id).First();
                model.UpdatedBy = item.user.Name;
            }

            return vehicleModels;
        }
        public IEnumerable<Brands> GetBrandsDropDown()
        {
            return _db.Brands;
        }
        public Database.Models GetModel(int id)
        {
            try
            {
                return _db.Models.Where(b => b.Id == id).FirstOrDefault();
            }
            catch (Exception)
            {
                //TODO: Add Logger
                throw;
            }
        }
        public string EditModel(Database.Models model, int loggedInUserId)
        {
            try
            {
                var modelFromDb = _db.Models.Where(u => u.Id == model.Id && u.IsDeleted == false).FirstOrDefault();

                bool doesModelCodeExists = false;
                //check if the new modelCode exists in the database
                if (modelFromDb.ModelCode != model.ModelCode)
                {
                    doesModelCodeExists = _db.Models.Any(m => m.ModelCode == model.ModelCode);
                }
                
                if (modelFromDb != null && !doesModelCodeExists)
                {
                    modelFromDb.Name = model.Name;
                    modelFromDb.BrandId = model.BrandId;
                    modelFromDb.FuelType = model.FuelType;
                    modelFromDb.TransmissionType = model.TransmissionType;
                    modelFromDb.ExteriorColor = model.ExteriorColor;
                    modelFromDb.InteriorColor = model.InteriorColor;
                    modelFromDb.ModelCode = model.ModelCode;
                    modelFromDb.UpdatedDate = DateTime.Now;
                    modelFromDb.UpdatedBy = loggedInUserId;
                    modelFromDb.IsActive = model.IsActive;
                    _db.Entry(modelFromDb).State = EntityState.Modified;
                    _db.SaveChanges();
                    return "Success";
                }
                return "Error occured";
            }
            catch (Exception e)
            {
                //TODO:Add logger.
                return "Error occured";
                throw;
            }
        }
        public string DeleteModel(int id, int loggedInUserId)
        {
            try
            {
                var modelFromDb = _db.Models.Where(u => u.Id == id && u.IsDeleted == false).FirstOrDefault();
                if (modelFromDb != null)
                {
                    modelFromDb.IsActive = false;
                    modelFromDb.IsDeleted = true;
                    modelFromDb.DeletedDate = DateTime.Now;
                    _db.Entry(modelFromDb).State = EntityState.Modified;
                    _db.SaveChanges();
                    return "Success";
                }
                return "Error occurred";
            }
            catch (Exception e)
            {
                //TODO:Add logger.
                return "Error occurred";
                throw;
            }
        }
    }
}

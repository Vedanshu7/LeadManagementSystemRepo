
using LMS.Web.DAL.Database;
using LMS.Web.DAL.Interface;
using LMS.Web.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace LMS.Web.DAL.Repository
{
    public class DealerRepository : IDealerRepository
    {
        private readonly LMSAzureEntities _db;
        public DealerRepository()
        {
            _db = new LMSAzureEntities();
        }

        public string CreateDealer(Dealers dealer, int loggedInUserId, List<int> brands)
        {
            try
            {
                var doesDealerExist = _db.Dealers.Any(d => d.DealerCode == dealer.DealerCode);
                if (!doesDealerExist)
                {
                    dealer.CreatedDate = DateTime.Now;
                    dealer.IsActive = true;
                    _db.Dealers.Add(dealer);
                    _db.SaveChanges();

                    var dealerIdFromDb = _db.Dealers.Where(d => d.DealerCode == dealer.DealerCode).First().Id;
                    
                    foreach (var item in brands)
                    {
                        var dealerBrandMapping = new DealerBrandMappings();
                        dealerBrandMapping.DealerId = dealerIdFromDb;
                        dealerBrandMapping.BrandId = item;
                        dealerBrandMapping.CreatedDate = DateTime.Now;
                        dealerBrandMapping.CreatedBy = loggedInUserId;
                        dealerBrandMapping.IsActive = true;
                        _db.DealerBrandMappings.Add(dealerBrandMapping);
                    }
                    
                    _db.SaveChanges();
                    return "Success";
                }
                return "Dealer already exists.";
            }
            catch (Exception)
            {
                //TODO: Add Logger
                throw;
            }
        }
        public List<DealerModel> GetDealers()
        {
            var dealers = _db.Dealers.Join(_db.Users,
                dealer => dealer.CreatedBy,
                user => user.Id,
                (dealer, user) => new { dealer, user }).Where(du => du.dealer.IsActive == true)
                .Select(du => du).ToList();

            var updatedDealers = _db.Dealers.Join(_db.Users,
                dealer => dealer.UpdatedBy,
                user => user.Id,
                (dealer, user) => new { dealer, user }).Where(du => du.dealer.IsActive == true)
                .Select(du => du).ToList();

            List<DealerModel> dealerModels = new List<DealerModel>();
            for (int i = 0; i < dealers.Count; i++)
            {
                //public int Id { get; set; }
                //public string Name { get; set; }
                //public string State { get; set; }
                //public string City { get; set; }
                //public string Pincode { get; set; }
                //public string DealerCode { get; set; }
                //public string SalesEmail { get; set; }
                //public string AfterSalesEmail { get; set; }
                //public DateTime CreatedDate { get; set; }
                //public DateTime? UpdatedDate { get; set; }
                //public string CreatedBy { get; set; }
                //public string UpdatedBy { get; set; }
                //public bool IsActive { get; set; }
                var dealerModel = new DealerModel();
                dealerModel.Id = dealers[i].dealer.Id;
                dealerModel.Name = dealers[i].dealer.Name;
                dealerModel.State = dealers[i].dealer.State;
                dealerModel.City = dealers[i].dealer.City;
                dealerModel.Pincode = dealers[i].dealer.Pincode;
                dealerModel.DealerCode = dealers[i].dealer.DealerCode;
                dealerModel.SalesEmail = dealers[i].dealer.SalesEmail;
                dealerModel.AfterSalesEmail = dealers[i].dealer.AfterSalesEmail;
                dealerModel.CreatedDate = dealers[i].dealer.CreatedDate;
                dealerModel.UpdatedDate = dealers[i].dealer.UpdatedDate;
                dealerModel.CreatedBy = dealers[i].user.Name;
                dealerModel.IsActive = dealers[i].dealer.IsActive;
                dealerModels.Add(dealerModel);
            }

            foreach (var item in updatedDealers)
            {
                var model = dealerModels.Where(v => v.Id == item.dealer.Id).First();
                model.UpdatedBy = item.user.Name;
            }

            return dealerModels;
        }
        public Dealers GetDealer(int id)
        {
            try
            {
                return _db.Dealers.Where(d => d.Id == id).FirstOrDefault();
            }
            catch (Exception)
            {
                //TODO: Add Logger
                throw;
            }
        }
        public string EditDealer(Dealers dealer, int loggedInUserId)
        {
            try
            {
                var dealerFromDb = _db.Dealers.Where(u => u.Id == dealer.Id && u.IsActive == true).FirstOrDefault();

                bool doesDealerCodeExists = false;
                //check if the new modelCode exists in the database
                if (dealerFromDb.DealerCode.Equals(dealer.DealerCode))
                {
                    doesDealerCodeExists = _db.Dealers.Any(m => m.DealerCode == dealer.DealerCode);
                }

                if (dealerFromDb != null && !doesDealerCodeExists)
                {
                    dealerFromDb.Name = dealer.Name;
                    dealerFromDb.State = dealer.State;
                    dealerFromDb.City = dealer.City;
                    dealerFromDb.Pincode = dealer.Pincode;
                    dealerFromDb.DealerCode = dealer.DealerCode;
                    dealerFromDb.SalesEmail = dealer.SalesEmail;
                    dealerFromDb.AfterSalesEmail = dealer.AfterSalesEmail;
                    dealerFromDb.UpdatedDate = DateTime.Now;
                    dealerFromDb.UpdatedBy = loggedInUserId;
                    dealerFromDb.IsActive = dealer.IsActive;
                    _db.Entry(dealerFromDb).State = EntityState.Modified;
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
    }
}

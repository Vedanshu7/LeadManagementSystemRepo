
using LMS.Web.DAL.Database;
using LMS.Web.DAL.Interface;
using LMS.Web.DAL.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace LMS.Web.DAL.Repository
{
    public class DealerRepository : IDealerRepository
    {
        private readonly LMSAzureEntities _db;
        private static readonly ILog Log = LogManager.GetLogger(typeof(DealerRepository));
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
            catch (Exception e)
            {
                Log.Error(e.Message, e);
                return "Error occurred";
            }
        }
        public List<DealerModel> GetDealers()
        {
            try
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
            catch (Exception e) { Log.Error(e.Message, e); return null; }
        }
        public Dealers GetDealer(int id)
        {
            try
            {
                return _db.Dealers.Where(d => d.Id == id).FirstOrDefault();
            }
            catch (Exception e) { Log.Error(e.Message, e); return null; }
        }
        public string EditDealer(Dealers dealer, int loggedInUserId, List<int> brands)
        {
            try
            {
                var dealerFromDb = _db.Dealers.Where(u => u.Id == dealer.Id && u.IsActive == true).FirstOrDefault();

                bool doesDealerCodeExists = false;
                //check if the new modelCode exists in the database
                if (!dealerFromDb.DealerCode.Equals(dealer.DealerCode))
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

                    //remove existing mappings
                    _db.DealerBrandMappings.RemoveRange(_db.DealerBrandMappings.Where(db => db.DealerId == dealer.Id).ToList());
                    _db.SaveChanges();

                    var dealerIdFromDb = dealer.Id;
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
                return "Error occured";
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e);
                return "Error occured";
            }
        }
    }
}

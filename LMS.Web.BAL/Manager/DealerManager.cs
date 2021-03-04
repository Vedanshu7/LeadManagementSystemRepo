
using AutoMapper;
using LMS.Web.BAL.Interface;
using LMS.Web.BAL.ViewModels;
using LMS.Web.DAL.Database;
using LMS.Web.DAL.Interface;
using LMS.Web.DAL.Models;
using System.Collections.Generic;

namespace LMS.Web.BAL.Manager
{
    public class DealerManager : IDealerManager
    {
        private readonly IDealerRepository _dealerRepository;
        public IMapper mapper;
        public MapperConfiguration config;

        public DealerManager(IDealerRepository dealerRepository)
        {
            _dealerRepository = dealerRepository;
            config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AdminDealerViewModel, Dealers>();
                cfg.CreateMap<Dealers, AdminDealerViewModel>();
                cfg.CreateMap<AdminDealerViewModel, DealerModel>();
                cfg.CreateMap<DealerModel, AdminDealerViewModel>();
            });
            mapper = config.CreateMapper();
        }
        public string CreateDealer(AdminDealerViewModel model, int loggedInUserId)
        {
            Dealers dealerToDb = mapper.Map<AdminDealerViewModel, Dealers>(model);
            dealerToDb.CreatedBy = loggedInUserId;
            return _dealerRepository.CreateDealer(dealerToDb, loggedInUserId, model.Brands);
        }
        public List<AdminDealerViewModel> GetDealers()
        {
            List<DealerModel> dealersFromDb = _dealerRepository.GetDealers();
            var adminDealers = mapper.Map<List<DealerModel>, List<AdminDealerViewModel>>(dealersFromDb);
            return adminDealers;
        }
        public AdminDealerViewModel GetDealer(int id)
        {
            var dealerFromDb = _dealerRepository.GetDealer(id);
            var dealer = mapper.Map<Dealers, AdminDealerViewModel>(dealerFromDb);
            dealer.Brands = new List<int>();
            foreach (var item in dealerFromDb.DealerBrandMappings)
            {
                dealer.Brands.Add(item.BrandId);
            }
            return dealer;
        }
        public string EditDealer(AdminDealerViewModel viewModel, int loggedInId)
        {
            Dealers model = mapper.Map<AdminDealerViewModel, Dealers>(viewModel);
            return _dealerRepository.EditDealer(model, loggedInId);
        }
    }
}

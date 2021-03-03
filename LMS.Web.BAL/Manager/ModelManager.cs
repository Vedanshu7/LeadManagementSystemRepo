using AutoMapper;
using LMS.Web.BAL.Interface;
using LMS.Web.BAL.ViewModels;
using LMS.Web.DAL.Interface;
using LMS.Web.DAL.Database;
using System;
using System.Collections.Generic;
using LMS.Web.DAL.Models;

namespace LMS.Web.BAL.Manager
{
    public class ModelManager : IModelManager
    {
        private readonly IModelRepository _modelRepository;
        public IMapper mapper;
        public MapperConfiguration config;

        public ModelManager(IModelRepository modelRepository)
        {
            _modelRepository = modelRepository;
            config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AdminModelViewModel, Models>();
                cfg.CreateMap<Models, AdminModelViewModel>();
                cfg.CreateMap<BrandViewModel, Brands>();
                cfg.CreateMap<Brands, BrandViewModel>();
                cfg.CreateMap<AdminModelViewModel, VehicleModel>();
                cfg.CreateMap<VehicleModel, AdminModelViewModel>();
            });
            mapper = config.CreateMapper();
        }

        public string CreateModel(AdminModelViewModel model, int loggedInUserId)
        {
            Models modelToDb = mapper.Map<AdminModelViewModel, Models>(model);
            modelToDb.CreatedBy = loggedInUserId;
            return _modelRepository.CreateModel(modelToDb, loggedInUserId);
        }
        public List<AdminModelViewModel> GetModelList()
        {
            List<VehicleModel> modelsFromDb = _modelRepository.GetModels();
            var adminModels = mapper.Map<List<VehicleModel>, List<AdminModelViewModel>>(modelsFromDb);
            return adminModels;
        }
        public IEnumerable<BrandViewModel> GetBrandsDropDown()
        {
            var brands = _modelRepository.GetBrandsDropDown();
            var brandViewModel = mapper.Map<IEnumerable<Brands>, IEnumerable<BrandViewModel>>(brands);
            return brandViewModel;
        }
        public AdminModelViewModel GetModel(int id)
        {
            var modelFromDb = _modelRepository.GetModel(id);
            var model = mapper.Map<Models, AdminModelViewModel>(modelFromDb);
            return model;
        }
        public string EditModel(AdminModelViewModel viewModel, int loggedInId)
        {
            Models model = mapper.Map<AdminModelViewModel, Models>(viewModel);
            return _modelRepository.EditModel(model, loggedInId);
        }

        public string DeleteModel(int id, int loggedInId)
        {
            return _modelRepository.DeleteModel(id, loggedInId);
        }
    }
}

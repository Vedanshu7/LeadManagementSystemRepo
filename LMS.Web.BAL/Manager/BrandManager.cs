using AutoMapper;
using LMS.Web.BAL.Interface;
using LMS.Web.BAL.ViewModels;
using LMS.Web.DAL.Database;
using LMS.Web.DAL.Interface;
using LMS.Web.DAL.Models;
using System.Collections.Generic;

namespace LMS.Web.BAL.Manager
{
    public class BrandManager : IBrandManager
    {
        public readonly IBrandRepository _brandRepository;
        public IMapper mapper;
        public MapperConfiguration config;

        public BrandManager(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
            config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AdminBrandViewModel, Brands>();
                cfg.CreateMap<Brands, AdminBrandViewModel>();
                cfg.CreateMap<VehicleBrand, Brands>();
                cfg.CreateMap<Brands, VehicleBrand>();
                cfg.CreateMap<AdminBrandViewModel, VehicleBrand>();
                cfg.CreateMap<VehicleBrand, AdminBrandViewModel>();

            });

            mapper = config.CreateMapper();
        }
        public string CreateBrand(AdminBrandViewModel model, int loggedInUserId)
        {
            Brands brands = mapper.Map<AdminBrandViewModel, Brands>(model);
            brands.CreatedBy = loggedInUserId;
            return _brandRepository.CreateBrand(brands);
        }
        public string EditBrand(AdminBrandViewModel model, int loggedInUserId)
        {
            Brands brands = mapper.Map<AdminBrandViewModel, Brands>(model);
            brands.UpdatedBy = loggedInUserId;
            return _brandRepository.EditBrand(brands);
        }
        public AdminBrandViewModel GetBrand(int id)
        {
            Brands brand = _brandRepository.GetBrand(id);
            AdminBrandViewModel model = mapper.Map<Brands, AdminBrandViewModel>(brand);
            return model;
        }
        public List<AdminBrandViewModel> GetBrandList()
        {
            List<VehicleBrand> brandsFromDb = _brandRepository.GetBrandList();
            List<AdminBrandViewModel> model = mapper.Map<List<VehicleBrand>, List<AdminBrandViewModel>>(brandsFromDb);
            return model;
        }
    }
}

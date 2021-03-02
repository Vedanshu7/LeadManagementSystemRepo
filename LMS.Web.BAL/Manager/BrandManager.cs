using AutoMapper;
using LMS.Web.BAL.Interface;
using LMS.Web.BAL.ViewModels;
using LMS.Web.DAL.Database;
using LMS.Web.DAL.Interface;
using LMS.Web.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                cfg.CreateMap<AdminBrandViewModel,Brands>();
                cfg.CreateMap<Brands, AdminBrandViewModel>();
                cfg.CreateMap<VehicleBrand, Brands>();
                cfg.CreateMap<Brands, VehicleBrand>();

            });

            mapper = config.CreateMapper();
        }
        public string CreateBrand(AdminBrandViewModel model, int loggedInUserId)
        {
            Brands brands = mapper.Map<AdminBrandViewModel,Brands>(model);
            brands.CreatedBy = loggedInUserId;
            return _brandRepository.CreateBrand(brands);
        }

        public List<AdminBrandViewModel> GetBrandList()
        {
            List<VehicleBrand> brandsFromDb = _brandRepository.GetBrandList();
            List<AdminBrandViewModel> model = mapper.Map<List<VehicleBrand>,List<AdminBrandViewModel>>(brandsFromDb);
            return model;

        }
    }
}
